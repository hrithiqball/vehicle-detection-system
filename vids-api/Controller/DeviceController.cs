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
    [Route("api/device")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private readonly IDeviceService _deviceService;

        public DeviceController(IDeviceService deviceService)
        {
            _deviceService = deviceService;
        }

        [HttpPost("add-device")]
        public async Task<IActionResult> AddDevice(
            string deviceName,
            string? deviceName2,
            string deviceTag,
            string ipAddress,
            string location,
            string bound,
            decimal km,
            decimal latitude,
            decimal longitude,
            string controlRoom,
            int total_lane,
            string? lane1Name,
            string? lane2Name,
            string? lane3Name,
            string? lane4Name,
            string? lane5Name,
            string? lane6Name,
            string congestionLine,
            int freeFlowSpeed,
            int roadCapacity,
            int totalClass,
            string? c1Name,
            string? c2Name,
            string? c3Name,
            string? c4Name,
            string? c5Name,
            string? c6Name,
            string? c7Name,
            bool? hasSpeed,
            bool? hasHeadway,
            bool? hasOccupancy,
            bool? hasGap,
            bool? hasFlowRate,
            bool? hasC1FlowRate,
            bool? hasC2FlowRate,
            bool? hasC3FlowRate,
            bool? hasC4FlowRate,
            bool? hasC5FlowRate,
            bool? hasC6FlowRate,
            bool? hasC7FlowRate,
            bool? hasLos,
            string ownerId
        )
        {
            Random rd = new Random();
            int rdnum = rd.Next(1, 2000);

            if (string.IsNullOrEmpty(deviceName))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return new JsonResult(
                    new Result { Status = ResultStatusValues.Error, Message = "Missing parameter" }
                );
            }

            try
            {
                Device device = new Device
                {
                    DeviceName = deviceName,
                    DeviceName2 = deviceName2,
                    DeviceTag = deviceTag,
                    IpAddress = ipAddress,
                    Location = location,
                    Bound = bound,
                    Km = km,
                    Latitude = latitude,
                    Longitude = longitude,
                    ControlRoom = controlRoom,
                    TotalLane = total_lane,
                    Lane1Id = "lane1_" + rdnum,
                    Lane1Name = lane1Name,
                    Lane2Id = "lane2_" + rdnum,
                    Lane2Name = lane2Name,
                    Lane3Id = "lane3_" + rdnum,
                    Lane3Name = lane3Name,
                    Lane4Id = "lane4_" + rdnum,
                    Lane4Name = lane4Name,
                    Lane5Id = "lane5_" + rdnum,
                    Lane5Name = lane5Name,
                    Lane6Id = "lane6_" + rdnum,
                    Lane6Name = lane6Name,
                    CongestionLine = congestionLine,
                    CameraId = "cam_" + rdnum,
                    FreeFlowSpeed = freeFlowSpeed,
                    RoadCapacity = roadCapacity,
                    TotalClass = totalClass,
                    C1Name = c1Name,
                    C2Name = c2Name,
                    C3Name = c3Name,
                    C4Name = c4Name,
                    C5Name = c5Name,
                    C6Name = c6Name,
                    C7Name = c7Name,
                    HasSpeed = hasSpeed,
                    HasHeadway = hasHeadway,
                    HasOccupancy = hasOccupancy,
                    HasGap = hasGap,
                    HasFlowRate = hasFlowRate,
                    HasC1FlowRate = hasC1FlowRate,
                    HasC2FlowRate = hasC2FlowRate,
                    HasC3FlowRate = hasC3FlowRate,
                    HasC4FlowRate = hasC4FlowRate,
                    HasC5FlowRate = hasC5FlowRate,
                    HasC6FlowRate = hasC6FlowRate,
                    HasC7FlowRate = hasC7FlowRate,
                    HasLos = hasLos,
                    OwnerId = ownerId,
                };

                Result result = await _deviceService.AddDeviceAsync(device);
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

        [HttpPost("update-lane-name")]
        public async Task<IActionResult> UpdateDevice(
            [FromForm] string deviceId,
            [FromForm] string lane1Name,
            [FromForm] string lane2Name,
            [FromForm] string lane3Name,
            [FromForm] string lane4Name,
            [FromForm] string lane5Name,
            [FromForm] string lane6Name
        )
        {
            try
            {
                Device device = new Device()
                {
                    DeviceId = deviceId,
                    Lane1Name = lane1Name,
                    Lane2Name = lane2Name,
                    Lane3Name = lane3Name,
                    Lane4Name = lane4Name,
                    Lane5Name = lane5Name,
                    Lane6Name = lane6Name
                };

                var result = await _deviceService.UpdateLaneNameAsync(
                    deviceId,
                    lane1Name,
                    lane2Name,
                    lane3Name,
                    lane4Name,
                    lane5Name,
                    lane6Name
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

        [HttpPost("update-class-name")]
        public async Task<IActionResult> UpdateClassName(
            [FromForm] string deviceId,
            [FromForm] string class1Name,
            [FromForm] string class2Name,
            [FromForm] string class3Name,
            [FromForm] string class4Name,
            [FromForm] string class5Name,
            [FromForm] string class6Name,
            [FromForm] string class7Name
        )
        {
            try
            {
                Device device = new Device()
                {
                    DeviceId = deviceId,
                    C1Name = class1Name,
                    C2Name = class2Name,
                    C3Name = class3Name,
                    C4Name = class4Name,
                    C5Name = class5Name,
                    C6Name = class6Name,
                    C7Name = class7Name
                };

                var result = await _deviceService.UpdateClassNameAsync(
                    deviceId,
                    class1Name,
                    class2Name,
                    class3Name,
                    class4Name,
                    class5Name,
                    class6Name,
                    class7Name
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

        [HttpGet("get-device")]
        public async Task<IActionResult> GetDevice(string deviceId)
        {
            try
            {
                var result = await _deviceService.GetDeviceAsync(deviceId);
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

        [HttpGet("get-device-list")]
        public async Task<IActionResult> GetDeviceList(
            int pageIndex = 0,
            int pageSize = 50,
            bool ascSort = false,
            string? controlRoom = null,
            int? totalLane = null,
            string? cameraId = null
        //string? deviceId = null,
        //string? laneId = null,
        //DateTime? passingTime = null,
        )
        {
            try
            {
                DeviceFilter filter = new DeviceFilter()
                {
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    AscSort = ascSort,
                    ControlRoom = controlRoom,
                    TotalLane = totalLane,
                    CameraId = cameraId
                    //DeviceId = deviceId,
                    //LaneId = laneId,
                    //PassingTime = passingTime,
                };

                var result = await _deviceService.GetDeviceListAsync(filter);
                if (result.Status == ResultStatusValues.OK)
                {
                    PageResult<List<Device>> pageResult = new PageResult<List<Device>>()
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
