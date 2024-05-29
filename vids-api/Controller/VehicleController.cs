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
    [Route("api/vehicle")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;

        public VehicleController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        [HttpPost("add-vehicle")]
        public async Task<IActionResult> AddVehicle(
            string laneId,
            string _class,
            string ownerId,
            int speed
        )
        {
            if (string.IsNullOrEmpty(laneId))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return new JsonResult(
                    new Result { Status = ResultStatusValues.Error, Message = "Missing parameter" }
                );
            }

            try
            {
                Vehicle vehicle = new Vehicle
                {
                    LaneId = laneId,
                    Class = _class,
                    OwnerId = ownerId,
                    Speed = speed
                };

                Result result = await _vehicleService.AddVehicleAsync(vehicle);
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

        [HttpGet("get-vehicle")]
        public async Task<IActionResult> GetVehicle(string id)
        {
            try
            {
                var result = await _vehicleService.GetVehicleAsync(id);
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

        //[HttpGet("get-vehicle-list")]
        //public async Task<IActionResult> GetVehicleList(
        //    int pageIndex = 0,
        //    int pageSize = 50,
        //    bool ascSort = false)
        //{
        //    try
        //    {
        //        VehicleFilter filter = new VehicleFilter();
        //        {
        //            PageIndex = pageIndex,
        //                PageSize = pageSize,
        //                AscSort = ascSort
        //        };

        //        var result = await _vehicleService.GetVehicleListAsync(filter);
        //        if (result.Status == ResultStatusValues.OK)
        //        {
        //            PageResult<List<Vehicle>> pageResult = new PageResult<List<Vehicle>>()
        //            {
        //                Data = result.Data,
        //                PageIndex = pageIndex,
        //                PageSize = pageSize
        //            };
        //            return new JsonResult(pageResult);
        //        }
        //        else
        //        {
        //            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        //            return new JsonResult(result);
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //        Response.StatusCode= (int)HttpStatusCode.InternalServerError;
        //        return new JsonResult(
        //            new Result()
        //            {
        //                Status = ResultStatusValues.Error,
        //                Message = ex.Message + "#" + ex.InnerException?.Message
        //            });
        //    }
        //}

        [HttpGet("get-vehicle-list")]
        public async Task<IActionResult> GetVehicleList(
            int pageIndex = 0,
            int pageSize = 50,
            bool ascSort = false,
            string? deviceId = null,
            string? laneId = null,
            DateTime? passingTime = null,
            int? speed = null,
            string? _class = null,
            string? ownerId = null
        )
        {
            try
            {
                VehicleFilter filter = new VehicleFilter()
                {
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    AscSort = ascSort,
                    DeviceId = deviceId,
                    LaneId = laneId,
                    PassingTime = passingTime,
                    Speed = speed,
                    Class = _class,
                    OwnerId = ownerId
                };

                var result = await _vehicleService.GetVehicleListAsync(filter);
                if (result.Status == ResultStatusValues.OK)
                {
                    PageResult<List<Vehicle>> pageResult = new PageResult<List<Vehicle>>()
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

        [HttpPost("update-vehicle")]
        public async Task<IActionResult> UpdateVehicle(
            [FromForm] string id,
            [FromForm] string deviceId,
            [FromForm] string laneId,
            [FromForm] DateTime? passingTime,
            [FromForm] int? speed,
            [FromForm] string _class,
            [FromForm] string ownerId
        )
        {
            try
            {
                Vehicle vehicle = new Vehicle()
                {
                    Id = id,
                    DeviceId = deviceId,
                    LaneId = laneId,
                    Speed = speed,
                    Class = _class,
                    OwnerId = ownerId
                };

                var result = await _vehicleService.UpdateVehicleAsync(vehicle);

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

        [HttpDelete("delete-vehicle")]
        public async Task<IActionResult> DeleteVehicle(string Id)
        {
            try
            {
                var result = await _vehicleService.DeleteVehicleAsync(Id);
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
    }
}
