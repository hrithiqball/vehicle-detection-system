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
    [Route("api/traffic-data")]
    [ApiController]
    public class TrafficDataController : ControllerBase
    {
        private readonly ITrafficDataService _trafficDataService;

        public TrafficDataController(ITrafficDataService trafficDataService)
        {
            _trafficDataService = trafficDataService;
        }

        [HttpPost("add-traffic-data")]
        public async Task<IActionResult> AddTrafficData(
            string deviceId,
            string laneId,
            int c1,
            int c2,
            int c3,
            int c4,
            int c5,
            int c6,
            int c7,
            int totalVol,
            int flowRate,
            int speed,
            int headway,
            string los,
            decimal gap,
            int c1FlowRate,
            int c2FlowRate,
            int c3FlowRate,
            int c4FlowRate,
            int c5FlowRate,
            int c6FlowRate,
            int c7FlowRate,
            string ownerId
        )
        {
            if (string.IsNullOrWhiteSpace(los))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return new JsonResult(
                    new Result { Status = ResultStatusValues.Error, Message = "Missing parameter" }
                );
            }

            try
            {
                TrafficData trafficData = new TrafficData
                {
                    DeviceId = deviceId,
                    LaneId = laneId,
                    C1 = c1,
                    C2 = c2,
                    C3 = c3,
                    C4 = c4,
                    C5 = c5,
                    C6 = c6,
                    C7 = c7,
                    TotalVol = totalVol,
                    FlowRate = flowRate,
                    Speed = speed,
                    Headway = headway,
                    Los = los,
                    Gap = gap,
                    C1FlowRate = c1FlowRate,
                    C2FlowRate = c2FlowRate,
                    C3FlowRate = c3FlowRate,
                    C4FlowRate = c4FlowRate,
                    C5FlowRate = c5FlowRate,
                    C6FlowRate = c6FlowRate,
                    C7FlowRate = c7FlowRate,
                    OwnerId = ownerId
                };

                Result result = await _trafficDataService.AddTrafficDataAsync(trafficData);
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
                    new Result()
                    {
                        Status = ResultStatusValues.Error,
                        Message = ex.Message + "#" + ex.InnerException?.Message
                    }
                );
            }
        }

        [HttpGet("get-traffic-data")]
        public async Task<IActionResult> GetTrafficData(string id)
        {
            try
            {
                var result = await _trafficDataService.GetTrafficDataAsync(id);
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

        [HttpGet("get-traffic-data-list")]
        public async Task<IActionResult> GetTrafficDataList(
            int pageIndex = 0,
            int pageSize = 50,
            bool ascSort = false,
            string? deviceId = null,
            string? laneId = null,
            DateTime? trafficTime = null,
            int? totalVol = null,
            int? flowRate = null,
            int? speed = null,
            int? headway = null,
            string? los = null,
            int? gap = null,
            string? ownerId = null
        )
        {
            try
            {
                TrafficDataFilter filter = new TrafficDataFilter()
                {
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    AscSort = ascSort,
                    DeviceId = deviceId,
                    LaneId = laneId,
                    TrafficTime = trafficTime,
                    TotalVol = totalVol,
                    FlowRate = flowRate,
                    Speed = speed,
                    Headway = headway,
                    Los = los,
                    Gap = gap,
                    OwnerId = ownerId
                };

                var result = await _trafficDataService.GetTrafficDataListAsync(filter);
                if (result.Status == ResultStatusValues.OK)
                {
                    PageResult<List<TrafficData>> pageResult = new PageResult<List<TrafficData>>()
                    {
                        Data = result.Data,
                        PageIndex = pageIndex,
                        PageSize = pageSize
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

        [HttpPost("update-flow-rate")]
        public async Task<IActionResult> UpdateFlowRate(
            [FromForm] string id,
            [FromForm] int c1FlowRate,
            [FromForm] int c2FlowRate,
            [FromForm] int c3FlowRate,
            [FromForm] int c4FlowRate,
            [FromForm] int c5FlowRate,
            [FromForm] int c6FlowRate,
            [FromForm] int c7FlowRate
        )
        {
            try
            {
                TrafficData trafficData = new TrafficData()
                {
                    Id = id,
                    C1FlowRate = c1FlowRate,
                    C2FlowRate = c2FlowRate,
                    C3FlowRate = c3FlowRate,
                    C4FlowRate = c4FlowRate,
                    C5FlowRate = c5FlowRate,
                    C6FlowRate = c6FlowRate,
                    C7FlowRate = c7FlowRate,
                };

                var result = await _trafficDataService.UpdateFlowRateAsync(
                    id,
                    c1FlowRate,
                    c2FlowRate,
                    c3FlowRate,
                    c4FlowRate,
                    c5FlowRate,
                    c6FlowRate,
                    c7FlowRate
                );

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

        [HttpDelete("delete-traffic-data")]
        public async Task<IActionResult> DeleteTrafficData(string Id)
        {
            try
            {
                var result = await _trafficDataService.DeleteTrafficDataAsync(Id);
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

        [HttpDelete("bulk-delete-traffic-data")]
        public async Task<IActionResult> BulkDeleteTrafficData(
            [FromBody] BulkDeleteTrafficData bulkDelete
        )
        {
            try
            {
                var result = await _trafficDataService.BulkDeleteTrafficDataAsync(
                    bulkDelete.ItemList
                );
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
                    new Result()
                    {
                        Status = ResultStatusValues.Error,
                        Message = ex.Message + "#" + ex.InnerException?.Message
                    }
                );
            }
        }

        //[HttpPost("update-traffic-data")]
        //public async Task<IActionResult> UpdateTrafficDataAsync(
        //    [FromForm] string los)
        //{
        //    try
        //    {
        //        var result = await _trafficDataService.UpdateTrafficDataAsync(
        //            los
        //            );

        //        if (result.Status == ResultStatusValues.OK)
        //        {
        //            return new JsonResult(result.Data);
        //        }
        //        else
        //        {
        //            Response.StatusCode= (int)HttpStatusCode.InternalServerError;
        //            return new JsonResult(result);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.StatusCode=(int)HttpStatusCode.InternalServerError;
        //        return new JsonResult(new Result()
        //        {
        //            Status = ResultStatusValues.Error,
        //            Message = ex.Message
        //        });
        //    }
        //}
    }
}
