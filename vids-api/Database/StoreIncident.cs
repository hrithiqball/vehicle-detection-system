using Vids.DbContexts.Postgres.VidsDb;
using Vids.Model;
using Vids.Service;
using System;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace Vids.Database
{
    public interface IStoreIncident
    {
        Task<Result<Incident>> AddIncident(string connectionStr, Incident incident);
        Task<Result<Incident>> GetIncident(string connectionStr, string id);
        Task<Result<List<Incident>>> GetIncidentList(string connectionStr, IncidentFilter filter);
    }

    public class StoreIncident : IStoreIncident
    {
        public StoreIncident() { }

        public async Task<Result<Incident>> AddIncident(string connectionStr, Incident incident)
        {
            Result<Incident> result = new Result<Incident>();

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
                    incident tmp = new incident
                    {
                        id = incident.Id,
                        device_id = incident.DeviceId,
                        lane_id = incident.LaneId,
                        incident_type = incident.IncidentType,
                        footage = incident.Footage,
                        owner_id = incident.OwnerId,
                    };

                    db.incident.Add(tmp);
                    result.Data = incident;
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

        public async Task<Result<Incident>> GetIncident(string connectionStr, string id)
        {
            Result<Incident> result = new Result<Incident>();

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
                    var target = await db.incident
                        .Where(t => t.id == id)
                        .Select(t => new Incident(t))
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

        public async Task<Result<List<Incident>>> GetIncidentList(
            string connectionStr,
            IncidentFilter filter
        )
        {
            Result<List<Incident>> result = new Result<List<Incident>>();
            result.Data = new List<Incident>();

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
                    IQueryable<incident> query = db.incident.AsQueryable();
                    query = Filter(query, filter);

                    if (filter.PageSize == 0)
                    {
                        result.Data = await query.Select(t => new Incident(t)).ToListAsync();
                    }
                    else
                    {
                        result.Data = await query
                            .Skip(filter.PageIndex * filter.PageSize)
                            .Take(filter.PageSize)
                            .Select(t => new Incident(t))
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

        private IQueryable<incident> Filter(IQueryable<incident> query, IncidentFilter filter)
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
                if (!string.IsNullOrEmpty(filter.IncidentType))
                {
                    query = query.Where(
                        t => t.incident_type.ToLower().Contains(filter.IncidentType.ToLower())
                    );
                }
                if (filter.StartTime.HasValue)
                {
                    query = query.Where(t => t.start_time >= filter.StartTime.Value);
                }
                if (filter.EndTime.HasValue)
                {
                    query = query.Where(t => t.end_time < filter.EndTime.Value);
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
                            return query.OrderBy(t => t.start_time);
                        }
                        else
                        {
                            return query.OrderByDescending(t => t.start_time);
                        }
                }
            }
            catch (Exception ex)
            {
                return query;
            }
        }
    }
}
