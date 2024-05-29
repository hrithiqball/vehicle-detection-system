using Vids.DbContexts.Postgres.VidsDb;
using Vids.Model;
using Vids.Service;
using System;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

namespace Vids.Database
{
    public interface IStoreTrafficData
    {
        Task<Result<TrafficData>> AddTrafficData(string connectionStr, TrafficData trafficData);
        Task<Result<TrafficData>> GetTrafficData(string connectionStr, string id);
        Task<Result<List<TrafficData>>> GetTrafficDataList(
            string connectionStr,
            TrafficDataFilter filter
        );
        Task<Result<ClassVolume>> UpdateClassVolume(
            string connectionStr,
            string id,
            int c1,
            int c2,
            int c3,
            int c4,
            int c5,
            int c6,
            int c7
        );
        Task<Result<FlowRate>> UpdateFlowRate(
            string connectionStr,
            string id,
            int c1FlowRate,
            int c2FlowRate,
            int c3FlowRate,
            int c4FlowRate,
            int c5FlowRate,
            int c6FlowRate,
            int c7FlowRate
        );
        Task<Result<TrafficData>> DeleteTrafficData(string connectionStr, string id);
        Task<Result<List<TrafficData>>> BulkDeleteTrafficData(
            string connectionStr,
            List<ItemInfo> itemlist
        );
        //Task<Result<TrafficData>> UpdateTrafficData(string connectionStr, TrafficData trafficData);
    }

    public class StoreTrafficData : IStoreTrafficData
    {
        public StoreTrafficData() { }

        public async Task<Result<TrafficData>> AddTrafficData(
            string connectionStr,
            TrafficData trafficData
        )
        {
            Result<TrafficData> result = new Result<TrafficData>();

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
                    traffic_data tmp = new traffic_data
                    {
                        id = trafficData.Id,
                        device_id = trafficData.DeviceId,
                        lane_id = trafficData.LaneId,
                        traffic_time = trafficData.TrafficTime,
                        c1 = trafficData.C1,
                        c2 = trafficData.C2,
                        c3 = trafficData.C3,
                        c4 = trafficData.C4,
                        c5 = trafficData.C5,
                        c6 = trafficData.C6,
                        c7 = trafficData.C7,
                        total_vol = trafficData.TotalVol,
                        flow_rate = trafficData.FlowRate,
                        speed = trafficData.Speed,
                        headway = trafficData.Headway,
                        los = trafficData.Los,
                        gap = trafficData.Gap,
                        c1_flow_rate = trafficData.C1FlowRate,
                        c2_flow_rate = trafficData.C2FlowRate,
                        c3_flow_rate = trafficData.C3FlowRate,
                        c4_flow_rate = trafficData.C4FlowRate,
                        c5_flow_rate = trafficData.C5FlowRate,
                        c6_flow_rate = trafficData.C6FlowRate,
                        c7_flow_rate = trafficData.C7FlowRate,
                        owner_id = trafficData.OwnerId,
                    };

                    db.traffic_data.Add(tmp);
                    result.Data = trafficData;
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

        public async Task<Result<TrafficData>> GetTrafficData(string connectionStr, string id)
        {
            Result<TrafficData> result = new Result<TrafficData>();

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
                    var target = await db.traffic_data
                        .Where(t => t.id == id)
                        .Select(t => new TrafficData(t))
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

        public async Task<Result<List<TrafficData>>> GetTrafficDataList(
            string connectionStr,
            TrafficDataFilter filter
        )
        {
            Result<List<TrafficData>> result = new Result<List<TrafficData>>();
            result.Data = new List<TrafficData>();

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
                    IQueryable<traffic_data> query = db.traffic_data.AsQueryable();
                    query = Filter(query, filter);

                    if (filter.PageSize == 0)
                    {
                        result.Data = await query.Select(t => new TrafficData(t)).ToListAsync();
                    }
                    else
                    {
                        result.Data = await query
                            .Skip(filter.PageIndex * filter.PageSize)
                            .Take(filter.PageSize)
                            .Select(t => new TrafficData(t))
                            .ToListAsync()
                            .ConfigureAwait(false);
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

        private IQueryable<traffic_data> Filter(
            IQueryable<traffic_data> query,
            TrafficDataFilter filter
        )
        {
            try
            {
                if (!string.IsNullOrEmpty(filter.DeviceId))
                {
                    query = query.Where(t => t.device_id.ToLower() == (filter.DeviceId.ToLower()));
                }
                if (!string.IsNullOrEmpty(filter.LaneId))
                {
                    query = query.Where(t => t.lane_id.ToLower() == (filter.LaneId.ToLower()));
                }
                if (filter.TrafficTime.HasValue)
                {
                    query = query.Where(t => t.traffic_time < filter.TrafficTime.Value);
                }
                if (!string.IsNullOrEmpty(filter.TotalVol.ToString()))
                {
                    query = query.Where(t => t.total_vol <= filter.TotalVol);
                }
                if (!string.IsNullOrEmpty(filter.FlowRate.ToString()))
                {
                    query = query.Where(t => t.flow_rate <= filter.FlowRate);
                }
                if (!string.IsNullOrEmpty(filter.Speed.ToString()))
                {
                    query = query.Where(t => t.speed <= filter.Speed);
                }
                if (!string.IsNullOrEmpty(filter.Headway.ToString()))
                {
                    query = query.Where(t => t.headway <= filter.Headway);
                }
                if (!string.IsNullOrEmpty(filter.Los))
                {
                    query = query.Where(t => t.los.ToLower() == (filter.Los.ToLower()));
                }
                if (!string.IsNullOrEmpty(filter.Gap.ToString()))
                {
                    query = query.Where(t => t.gap <= filter.Gap);
                }
                if (!string.IsNullOrEmpty(filter.OwnerId))
                {
                    query = query.Where(t => t.owner_id.ToLower() == (filter.OwnerId.ToLower()));
                }

                switch (filter.SortBy)
                {
                    case "time":
                    default:
                        if (filter.AscSort)
                        {
                            return query.OrderBy(t => t.traffic_time);
                        }
                        else
                        {
                            return query.OrderByDescending(t => t.traffic_time);
                        }
                }
            }
            catch (Exception ex)
            {
                return query;
            }
        }

        public async Task<Result<FlowRate>> UpdateFlowRate(
            string connectionStr,
            string id,
            int c1FlowRate,
            int c2FlowRate,
            int c3FlowRate,
            int c4FlowRate,
            int c5FlowRate,
            int c6FlowRate,
            int c7FlowRate
        )
        {
            Result<FlowRate> result = new Result<FlowRate>();

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
                    var target = await db.traffic_data.Where(t => t.id == id).FirstOrDefaultAsync();
                    if (target != null)
                    {
                        target.c1_flow_rate = c1FlowRate;
                        target.c2_flow_rate = c2FlowRate;
                        target.c3_flow_rate = c3FlowRate;
                        target.c4_flow_rate = c4FlowRate;
                        target.c5_flow_rate = c5FlowRate;
                        target.c6_flow_rate = c6FlowRate;
                        target.c7_flow_rate = c7FlowRate;
                    }
                    else
                    {
                        result.Status = ResultStatusValues.Error;
                        result.Message = "Not Exist";
                    }
                    result.Data = new FlowRate(target);
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

        public async Task<Result<ClassVolume>> UpdateClassVolume(
            string connectionStr,
            string id,
            int c1,
            int c2,
            int c3,
            int c4,
            int c5,
            int c6,
            int c7
        )
        {
            Result<ClassVolume> result = new Result<ClassVolume>();

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
                    var target = await db.traffic_data.Where(t => t.id == id).FirstOrDefaultAsync();
                    if (target != null)
                    {
                        target.c1 = c1;
                        target.c2 = c2;
                        target.c3 = c3;
                        target.c4 = c4;
                        target.c5 = c5;
                        target.c6 = c6;
                        target.c7 = c7;
                    }
                    else
                    {
                        result.Status = ResultStatusValues.Error;
                        result.Message = "Not Exist";
                    }
                    result.Data = new ClassVolume(target);
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

        public async Task<Result<TrafficData>> DeleteTrafficData(string connectionStr, string id)
        {
            Result<TrafficData> result = new Result<TrafficData>();

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
                    var target = await db.traffic_data.Where(t => t.id == id).FirstOrDefaultAsync();

                    if (target != null)
                    {
                        result.Data = new TrafficData(target);

                        db.traffic_data.Remove(target);
                        await db.SaveChangesAsync();
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

        public async Task<Result<List<TrafficData>>> BulkDeleteTrafficData(
            string connectionStr,
            List<ItemInfo> itemList
        )
        {
            Result<List<TrafficData>> result = new Result<List<TrafficData>>();
            result.Data = new List<TrafficData>();

            try
            {
                if (string.IsNullOrEmpty(connectionStr))
                {
                    result.Status = ResultStatusValues.Error;
                    result.Message = "Db offline";
                    return result;
                }

                using (var db = new VidsDbContext(connectionStr))
                {
                    foreach (var item in itemList)
                    {
                        var target = await db.traffic_data
                            .Where(t => t.id == item.Id)
                            .FirstOrDefaultAsync();
                        if (target != null)
                        {
                            var trafficData = new TrafficData(target);
                            result.Data.Add(trafficData);

                            db.traffic_data.Remove(target);
                        }
                    }
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

        //public async Task<Result<TrafficData>> UpdateTrafficData(string connectionStr, TrafficData trafficData)
        //{
        //    Result<TrafficData> result = new Result<TrafficData>();

        //    try
        //    {
        //        if (string.IsNullOrEmpty(connectionStr))
        //        {
        //            result.Status = ResultStatusValues.Error;
        //            result.Message = "Db Offline";
        //            return result;
        //        }

        //        using (var db = new VidsDbContext(connectionStr))
        //        {
        //            var target = await db.traffic_data.Where(t => t.id == trafficData.Id).FirstOrDefaultAsync();

        //            if (target!= null)
        //            {
        //                target.total_vol = trafficData.TotalVol;
        //                target.c1 = trafficData.C1;
        //                target.c2 = trafficData.C2;
        //                target.c3 = trafficData.C3;
        //                target.c4 = trafficData.C4;
        //                target.c5 = trafficData.C5;
        //                target.c6 = trafficData.C6;
        //                target.c7 = trafficData.C7;
        //                target.c1_flow_rate = trafficData.C1FlowRate;
        //                target.c2_flow_rate= trafficData.C2FlowRate;
        //                target.c3_flow_rate= trafficData.C3FlowRate;
        //                target.c4_flow_rate= trafficData.C4FlowRate;
        //                target.c5_flow_rate= trafficData.C5FlowRate;
        //                target.c6_flow_rate= trafficData.C6FlowRate;
        //                target.c7_flow_rate= trafficData.C7FlowRate;
        //                target.total_vol = trafficData.TotalVol;
        //                target.device_id= trafficData.DeviceId;
        //                target.lane_id= trafficData.LaneId;
        //                target.flow_rate= trafficData.FlowRate;
        //                target.speed= trafficData.Speed;
        //                target.headway= trafficData.Headway;
        //                target.los = trafficData.Los;
        //                target.gap= trafficData.Gap;
        //            }
        //            else
        //            {
        //                result.Status= ResultStatusValues.Error;
        //                result.Message = "Not exist";
        //            }

        //            await db.SaveChangesAsync();
        //        }

        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        result.Status = ResultStatusValues.Error;
        //        result.Message = ex.Message + "#" + ex.InnerException?.Message;
        //        return result;
        //    }
        //}
    }
}
