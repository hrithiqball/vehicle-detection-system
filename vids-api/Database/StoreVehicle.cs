using Vids.DbContexts.Postgres.VidsDb;
using Vids.Model;
using Vids.Service;
using System;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

namespace Vids.Database
{
    public interface IStoreVehicle
    {
        Task<Result<Vehicle>> AddVehicle(string connectionStr, Vehicle vehicle);
        Task<Result<Vehicle>> GetVehicle(string connectionStr, string id);
        Task<Result<List<Vehicle>>> GetVehicleList(string connectionStr, VehicleFilter filter);
        Task<Result<Vehicle>> UpdateVehicle(string connectionStr, Vehicle vehicle);
        Task<Result<Vehicle>> DeleteVehicle(string connectionStr, string id);
    }

    public class StoreVehicle : IStoreVehicle
    {
        public StoreVehicle() { }

        public async Task<Result<Vehicle>> AddVehicle(string connectionStr, Vehicle vehicle)
        {
            Result<Vehicle> result = new Result<Vehicle>();
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
                    vehicle tmp = new vehicle
                    {
                        id = vehicle.Id,
                        lane_id = vehicle.LaneId,
                        passing_time = vehicle.PassingTime,
                        speed = vehicle.Speed,
                        _class = vehicle.Class,
                        owner_id = vehicle.OwnerId,
                    };

                    db.vehicle.Add(tmp);
                    result.Data = vehicle;
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

        public async Task<Result<Vehicle>> GetVehicle(string connectionStr, string id)
        {
            Result<Vehicle> result = new Result<Vehicle>();

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
                    var target = await db.vehicle
                        .Where(t => t.id == id)
                        .Select(t => new Vehicle(t))
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

        public async Task<Result<List<Vehicle>>> GetVehicleList(
            string connectionStr,
            VehicleFilter filter
        )
        {
            Result<List<Vehicle>> result = new Result<List<Vehicle>>();
            result.Data = new List<Vehicle>();

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
                    IQueryable<vehicle> query = db.vehicle.AsQueryable();
                    query = Filter(query, filter);

                    if (filter.PageSize == 0)
                    {
                        result.Data = await query.Select(t => new Vehicle(t)).ToListAsync();
                    }
                    else
                    {
                        result.Data = await query
                            .Skip(filter.PageIndex * filter.PageSize)
                            .Take(filter.PageSize)
                            .Select(t => new Vehicle(t))
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

        private IQueryable<vehicle> Filter(IQueryable<vehicle> query, VehicleFilter filter)
        {
            try
            {
                if (!string.IsNullOrEmpty(filter.DeviceId))
                {
                    query = query.Where(t => t.device_id.ToLower() == filter.DeviceId.ToLower());
                }
                if (!string.IsNullOrEmpty(filter.LaneId))
                {
                    query = query.Where(t => t.lane_id.ToLower() == filter.LaneId.ToLower());
                }
                if (filter.PassingTime.HasValue)
                {
                    query = query.Where(t => t.passing_time >= filter.PassingTime.Value);
                }
                if (!string.IsNullOrEmpty(filter.Speed.ToString()))
                {
                    query = query.Where(t => t.speed <= filter.Speed);
                }
                if (!string.IsNullOrEmpty(filter.Class))
                {
                    query = query.Where(t => t._class.ToLower() == filter.Class.ToLower());
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
                            return query.OrderBy(t => t.passing_time);
                        }
                        else
                        {
                            return query.OrderByDescending(t => t.passing_time);
                        }
                }
            }
            catch (Exception ex)
            {
                return query;
            }
        }

        public async Task<Result<Vehicle>> UpdateVehicle(string connectionStr, Vehicle vehicle)
        {
            Result<Vehicle> result = new Result<Vehicle>();

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
                    var target = await db.vehicle
                        .Where(t => t.id == vehicle.Id)
                        .FirstOrDefaultAsync();
                    //var something = await db.device.Where(x => x.ownerId == device.OwnerId).FirstOrDefaultAsync();

                    if (target != null)
                    {
                        target.device_id = vehicle.DeviceId;
                        target.lane_id = vehicle.LaneId;
                        target.speed = vehicle.Speed;
                        target._class = vehicle.Class;
                        target.owner_id = vehicle.OwnerId;
                    }
                    else
                    {
                        result.Status = ResultStatusValues.Error;
                        result.Message = "Not Exist";
                    }
                    result.Data = vehicle;
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

        public async Task<Result<Vehicle>> DeleteVehicle(string connectionStr, string id)
        {
            Result<Vehicle> result = new Result<Vehicle>();

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
                    var target = await db.vehicle.Where(t => t.id == id).FirstOrDefaultAsync();

                    if (target != null)
                    {
                        result.Data = new Vehicle(target);

                        db.vehicle.Remove(target);
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
    }
}
