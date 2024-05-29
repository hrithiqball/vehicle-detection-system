using System.Net;
using Vids.Attributes;
using Vids.Configuration;
using Vids.Model;
using Vids.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Vids.Controller
{
    [ApiKey]
    [Route("api/incident")]
    [ApiController]
    public class IncidentController : ControllerBase
    {
        private readonly IIncidentService _incidentService;

        public IncidentController(IIncidentService incidentService)
        {
            _incidentService = incidentService;
        }

        [HttpPost("add-incident")]
        public async Task<IActionResult> AddIncident(
            string deviceId,
            string laneId,
            string incidentType,
            bool footage,
            string ownerId
        )
        {
            if (string.IsNullOrEmpty(incidentType))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return new JsonResult(
                    new Result { Status = ResultStatusValues.Error, Message = "Missing parameter" }
                );
            }

            try
            {
                Incident incident = new Incident
                {
                    DeviceId = deviceId,
                    LaneId = laneId,
                    IncidentType = incidentType,
                    Footage = footage,
                    OwnerId = ownerId
                };

                Result result = await _incidentService.AddIncidentAsync(incident);
                if (result.Status == ResultStatusValues.OK)
                {
                    return new JsonResult(result);
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    return new JsonResult(result);
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return new JsonResult(
                    new Result { Status = ResultStatusValues.Error, Message = ex.Message }
                );
            }
        }

        [HttpGet("get-incident")]
        public async Task<IActionResult> GetIncident(string id)
        {
            try
            {
                var result = await _incidentService.GetIncidentAsync(id);
                if (result.Status == ResultStatusValues.OK)
                {
                    return new JsonResult(result.Data);
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    return new JsonResult(result);
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return new JsonResult(
                    new Result()
                    {
                        Status = ResultStatusValues.Error,
                        Message = ex.Message + "#" + ex.InnerException?.Message
                    }
                );
            }
        }

        [HttpGet("get-incident-list")]
        public async Task<IActionResult> GetIncidentList(
            int pageIndex = 0,
            int pageSize = 50,
            bool ascSort = false,
            string? deviceId = null,
            string? laneId = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            string? incidentType = null,
            string? sortBy = null
        )
        {
            try
            {
                IncidentFilter filter = new IncidentFilter()
                {
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    AscSort = ascSort,
                    DeviceId = deviceId,
                    LaneId = laneId,
                    StartTime = startTime,
                    EndTime = endTime,
                    IncidentType = incidentType,
                    SortBy = sortBy
                };

                var result = await _incidentService.GetIncidentListAsync(filter);
                if (result.Status == ResultStatusValues.OK)
                {
                    PageResult<List<Incident>> pageResult = new PageResult<List<Incident>>()
                    {
                        Data = result.Data,
                        PageIndex = pageIndex,
                        PageSize = pageSize,
                    };
                    return new JsonResult(pageResult);
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    return new JsonResult(result);
                }
            }
            catch (System.Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return new JsonResult(
                    new Result()
                    {
                        Status = ResultStatusValues.Error,
                        Message = ex.Message + "#" + ex.InnerException?.Message
                    }
                );
            }
        }
    }
}
