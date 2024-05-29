using System.Diagnostics;
using System.Reflection;

using Vids.Configuration;
using Vids.Database;
using Vids.Model;
//using Vids.Models;
using Vids.Service;

using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;

//NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddCors(o => o.AddPolicy("AllowAnyCorsPolicy", builder =>
//{
//    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
//}));
// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Configure Services
//Device
builder.Services.AddSingleton(typeof(IStoreDevice), typeof(StoreDevice));
builder.Services.AddSingleton(typeof(IDeviceService), typeof(DeviceService));

//Incident
builder.Services.AddSingleton(typeof(IStoreIncident), typeof(StoreIncident));
builder.Services.AddSingleton(typeof(IIncidentService), typeof(IncidentService));

//Traffic Data
builder.Services.AddSingleton(typeof(IStoreTrafficData), typeof(StoreTrafficData));
builder.Services.AddSingleton(typeof(ITrafficDataService), typeof(TrafficDataService));

//Vehicle
builder.Services.AddSingleton(typeof(IStoreVehicle), typeof(StoreVehicle));
builder.Services.AddSingleton(typeof(IVehicleService), typeof(VehicleService));

builder.Services.AddSingleton(typeof(IDbService), typeof(DbService));
builder.Services.AddSingleton(typeof(INlogger), typeof(Nlogger));
#endregion

var app = builder.Build();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//useCors
app.UseCors();
app.UseCors(
    x => x.SetIsOriginAllowed(t => true).AllowAnyMethod().AllowAnyHeader().AllowCredentials()
);

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
