using Microsoft.AspNetCore.Mvc.Filters;
using Vids.Configuration;
using Vids.Service;

namespace Vids.Attributes
{
    [AttributeUsage(validOn: AttributeTargets.Class | AttributeTargets.Method)] //this attribute only valid on class or method
    public class ApiKeyAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var logger = context.HttpContext.RequestServices.GetRequiredService<INlogger>();

            try
            {
                if (!context.HttpContext.Request.Headers.TryGetValue("api-key", out var extractedApiKey))
                {
                    context.HttpContext.Response.StatusCode = 401;
                    await context.HttpContext.Response.WriteAsync("Api key is not provided.");
                    return;
                }

                var appSettings = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
                var apiKeyConfig = appSettings.GetSection("ApiKeys").Get<ApiKeyConfig>();
                
                bool anyKeyMatched = apiKeyConfig.Keys.Any(t => t.Key == extractedApiKey);

                if (!anyKeyMatched)
                {
                    context.HttpContext.Response.StatusCode = 401;
                    await context.HttpContext.Response.WriteAsync("Unauthorized client");
                    return;
                }
                else
                {
                    var origin = context.HttpContext.Request.Headers["origin"].FirstOrDefault();

                    var allowedDomains = apiKeyConfig.Keys[extractedApiKey].Split(";");

                    // * is allow all domain. separate the domain by ;. the api key is configured in appsetting.Development.json/appsetting.json.

                    if (!allowedDomains.Any(t => t == "*"))
                    {
                        if (string.IsNullOrEmpty(origin))
                        {
                            context.HttpContext.Response.StatusCode = 401;
                            await context.HttpContext.Response.WriteAsync("Origin is not provided.");
                            return;
                        }
                        else
                        {
                            if (!allowedDomains.Any(t => t == origin))
                            {
                                context.HttpContext.Response.StatusCode = 401;
                                await context.HttpContext.Response.WriteAsync("Origin not matched.");
                                return;
                            }
                        }
                    }
                }

                await next();

            }
            catch (System.Exception ex)
            {
                logger.Error(ex);
            }
        }
    }
}
