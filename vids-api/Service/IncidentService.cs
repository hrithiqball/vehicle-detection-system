using NLog.Filters;
using Vids.Database;
using Vids.Model;

namespace Vids.Service
{
    public interface IIncidentService
    {
        Task<Result<Incident>> AddIncidentAsync(Incident incident);
        Task<Result<Incident>> GetIncidentAsync(string id);
        Task<Result<List<Incident>>> GetIncidentListAsync(IncidentFilter filter);
    }

    public class IncidentService : IIncidentService
    {
        private readonly IDbService _dbService;
        private readonly IStoreIncident _storeIncident;

        public IncidentService(IDbService dbService, IStoreIncident storeIncident)
        {
            _storeIncident = storeIncident;
            _dbService = dbService;
        }

        public async Task<Result<Incident>> AddIncidentAsync(Incident incident)
        {
            try
            {
                Random rd = new Random();
                int rnum = rd.Next(1, 20000);
                incident.Id = rnum + DateTime.Now.ToString("HHmmssfff");

                var result = await _storeIncident.AddIncident(
                    _dbService.AppDbConnectionStr,
                    incident
                );
                return result;
            }
            catch (Exception ex)
            {
                return new Result<Incident>
                {
                    Status = ResultStatusValues.Error,
                    Message = ex.Message + "#" + ex.InnerException?.Message,
                };
            }
        }

        public async Task<Result<Incident>> GetIncidentAsync(string id)
        {
            try
            {
                var result = await _storeIncident.GetIncident(_dbService.AppDbConnectionStr, id);
                return result;
            }
            catch (Exception ex)
            {
                return new Result<Incident>
                {
                    Status = ResultStatusValues.Error,
                    Message = ex.Message + "#" + ex.InnerException?.Message
                };
            }
        }

        public async Task<Result<List<Incident>>> GetIncidentListAsync(IncidentFilter filter)
        {
            try
            {
                var result = await _storeIncident.GetIncidentList(
                    _dbService.AppDbConnectionStr,
                    filter
                );
                return result;
            }
            catch (Exception ex)
            {
                return new Result<List<Incident>>()
                {
                    Status = ResultStatusValues.Error,
                    Message = ex.Message + "#" + ex.InnerException?.Message
                };
            }
        }
    }
}
