using Vids.DbContexts.Postgres.VidsDb;
using Vids.Model;
using Vids.Service;
using System;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace Vids.Database
{
    public interface IStoreDevice
    {
        Task<Result<Device>> AddDevice(string connectionStr, Device device);
        Task<Result<Device>> GetDevice(string connectionStr, string deviceId);
        Task<Result<List<Device>>> GetDeviceList(string connectionStr, DeviceFilter filter);
        Task<Result<LaneName>> UpdateLaneName(
            string connectionsStr,
            string deviceId,
            string lane1Name,
            string lane2Name,
            string lane3Name,
            string lane4Name,
            string lane5Name,
            string lane6Name
        );

        Task<Result<ClassName>> UpdateClassName(
            string connectionStr,
            string deviceId,
            string c1Name,
            string c2Name,
            string c3Name,
            string c4Name,
            string c5Name,
            string c6Name,
            string c7Name
        );
    }

    public class StoreDevice : IStoreDevice
    {
        public StoreDevice() { }

        public async Task<Result<Device>> AddDevice(string connectionStr, Device device)
        {
            Result<Device> result = new Result<Device>();

            try
            {
                if (string.IsNullOrEmpty(connectionStr))
                {
                    result.Status = ResultStatusValues.Error;
                    result.Message = "Db Offline";
                    return result;
                }

                using (VidsDbContext db = new VidsDbContext(connectionStr))
                {
                    device tmp = new device
                    {
                        device_id = device.DeviceId,
                        device_name = device.DeviceName,
                        device_name_2 = device.DeviceName2,
                        device_tag = device.DeviceTag,
                        ip_address = device.IpAddress,
                        location = device.Location,
                        bound = device.Bound,
                        km = device.Km,
                        latitude = device.Latitude,
                        longitude = device.Longitude,
                        control_room = device.ControlRoom,
                        total_lane = device.TotalLane,
                        lane1_id = device.Lane1Id,
                        lane1_name = device.Lane1Name,
                        lane2_id = device.Lane2Id,
                        lane2_name = device.Lane2Name,
                        lane3_id = device.Lane3Id,
                        lane3_name = device.Lane3Name,
                        lane4_id = device.Lane4Id,
                        lane4_name = device.Lane4Name,
                        lane5_id = device.Lane5Id,
                        lane5_name = device.Lane5Name,
                        lane6_id = device.Lane6Id,
                        lane6_name = device.Lane6Name,
                        congestion_line = device.CongestionLine,
                        camera_id = device.CameraId,
                        free_flow_speed = device.FreeFlowSpeed,
                        road_capacity = device.RoadCapacity,
                        total_class = device.TotalClass,
                        c1_name = device.C1Name,
                        c2_name = device.C2Name,
                        c3_name = device.C3Name,
                        c4_name = device.C4Name,
                        c5_name = device.C5Name,
                        c6_name = device.C6Name,
                        c7_name = device.C7Name,
                        has_speed = device.HasSpeed,
                        has_headway = device.HasHeadway,
                        has_occupancy = device.HasOccupancy,
                        has_gap = device.HasGap,
                        has_flow_rate = device.HasFlowRate,
                        has_c1_flow_rate = device.HasC1FlowRate,
                        has_c2_flow_rate = device.HasC2FlowRate,
                        has_c3_flow_rate = device.HasC3FlowRate,
                        has_c4_flow_rate = device.HasC4FlowRate,
                        has_c5_flow_rate = device.HasC5FlowRate,
                        has_c6_flow_rate = device.HasC6FlowRate,
                        has_c7_flow_rate = device.HasC7FlowRate,
                        has_los = device.HasLos,
                        owner_id = device.OwnerId,
                        created_time = device.CreatedTime,
                    };

                    db.device.Add(tmp);
                    result.Data = device;
                    await db.SaveChangesAsync();
                }

                return result;
            }
            catch (Exception ex)
            {
                result.Status = ResultStatusValues.Error;
                result.Message = ex.Message;
                return result;
            }
        }

        public async Task<Result<Device>> GetDevice(string connectionStr, string deviceId)
        {
            Result<Device> result = new Result<Device>();

            try
            {
                if (string.IsNullOrEmpty(connectionStr))
                {
                    result.Status = ResultStatusValues.Error;
                    result.Message = "Db Offline";
                    return result;
                }

                using (VidsDbContext db = new VidsDbContext(connectionStr))
                {
                    var target = await db.device
                        .Where(t => t.device_id == deviceId)
                        .Select(t => new Device(t))
                        .FirstOrDefaultAsync();

                    if (target != null)
                    {
                        result.Data = target;
                    }
                    else
                    {
                        result.Status = ResultStatusValues.Error;
                        result.Message = "Not Exist";
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                result.Status = ResultStatusValues.Error;
                result.Message = ex.Message + "#" + ex.InnerException?.Message;
                return result;
            }
        }

        public async Task<Result<List<Device>>> GetDeviceList(
            string connectionStr,
            DeviceFilter filter
        )
        {
            Result<List<Device>> result = new Result<List<Device>>();
            result.Data = new List<Device>();

            try
            {
                if (string.IsNullOrEmpty(connectionStr))
                {
                    result.Status = ResultStatusValues.Error;
                    result.Message = "DB Offline";
                    return result;
                }

                using (var db = new VidsDbContext(connectionStr))
                {
                    IQueryable<device> query = db.device.AsQueryable();
                    query = Filter(query, filter);

                    if (filter.PageSize == 0)
                    {
                        result.Data = await query.Select(t => new Device(t)).ToListAsync();
                    }
                    else
                    {
                        result.Data = await query
                            .Skip(filter.PageIndex * filter.PageSize)
                            .Take(filter.PageSize)
                            .Select(t => new Device(t))
                            .ToListAsync();
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                result.Status = ResultStatusValues.Error;
                result.Message = ex.Message + "#" + ex.InnerException?.Message;
                return result;
            }
        }

        private IQueryable<device> Filter(IQueryable<device> query, DeviceFilter filter)
        {
            try
            {
                if (!string.IsNullOrEmpty(filter.TotalLane.ToString()))
                {
                    query = query.Where(t => t.total_lane == filter.TotalLane);
                }
                if (!string.IsNullOrEmpty(filter.ControlRoom))
                {
                    query = query.Where(t => t.control_room == filter.ControlRoom);
                }
                if (string.IsNullOrEmpty(filter.CameraId))
                {
                    query = query.Where(t => t.camera_id == filter.CameraId);
                }

                switch (filter.SortBy)
                {
                    case "time":
                    default:
                        if (filter.AscSort)
                        {
                            return query.OrderBy(t => t.created_time);
                        }
                        else
                        {
                            return query.OrderByDescending(t => t.created_time);
                        }
                }
            }
            catch (Exception ex)
            {
                return query;
            }
        }

        public async Task<Result<LaneName>> UpdateLaneName(
            string connectionStr,
            string deviceId,
            string lane1Name,
            string lane2Name,
            string lane3Name,
            string lane4Name,
            string lane5Name,
            string lane6Name
        )
        {
            Result<LaneName> result = new Result<LaneName>();

            try
            {
                if (string.IsNullOrEmpty(connectionStr))
                {
                    result.Status = ResultStatusValues.Error;
                    result.Message = "Db Offline";
                    return result;
                }

                using (var db = new VidsDbContext(connectionStr))
                {
                    var target = await db.device
                        .Where(t => t.device_id == deviceId)
                        .FirstOrDefaultAsync();
                    if (target != null)
                    {
                        target.lane1_name = lane1Name;
                        target.lane2_name = lane2Name;
                        target.lane3_name = lane3Name;
                        target.lane4_name = lane4Name;
                        target.lane5_name = lane5Name;
                        target.lane6_name = lane6Name;
                    }
                    else
                    {
                        result.Status = ResultStatusValues.Error;
                        result.Message = "Not exist";
                    }
                    result.Data = new LaneName(target);
                    await db.SaveChangesAsync();
                }
                return result;
            }
            catch (Exception ex)
            {
                result.Status = ResultStatusValues.Error;
                result.Message = ex.Message + "#" + ex.InnerException?.Message;
                return result;
            }
        }

        public async Task<Result<ClassName>> UpdateClassName(
            string connectionStr,
            string deviceId,
            string c1Name,
            string c2Name,
            string c3Name,
            string c4Name,
            string c5Name,
            string c6Name,
            string c7Name
        )
        {
            Result<ClassName> result = new Result<ClassName>();

            try
            {
                if (string.IsNullOrEmpty(connectionStr))
                {
                    result.Status = ResultStatusValues.Error;
                    result.Message = "Db Offline";
                    return result;
                }

                using (var db = new VidsDbContext(connectionStr))
                {
                    var target = await db.device
                        .Where(t => t.device_id == deviceId)
                        .FirstOrDefaultAsync();

                    if (target != null)
                    {
                        target.c1_name = c1Name;
                        target.c2_name = c2Name;
                        target.c3_name = c3Name;
                        target.c4_name = c4Name;
                        target.c5_name = c5Name;
                        target.c6_name = c6Name;
                        target.c7_name = c7Name;
                    }
                    else
                    {
                        result.Status = ResultStatusValues.Error;
                        result.Message = "Not exist";
                    }
                    result.Data = new ClassName(target);
                    await db.SaveChangesAsync();
                }
                return result;
            }
            catch (Exception ex)
            {
                result.Status = ResultStatusValues.Error;
                result.Message = ex.Message + "#" + ex.InnerException?.Message;
                return result;
            }
        }
    }
}
