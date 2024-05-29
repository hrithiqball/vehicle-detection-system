using Vids.Database;
using Vids.DbContexts.Postgres.VidsDb;
using Vids.Model;

namespace Vids.Service
{
    public interface ITrafficDataService
    {
        Task<Result<TrafficData>> AddTrafficDataAsync(TrafficData trafficData);
        Task<Result<TrafficData>> GetTrafficDataAsync(string id);
        Task<Result<List<TrafficData>>> GetTrafficDataListAsync(TrafficDataFilter filter);
        Task<Result<FlowRate>> UpdateFlowRateAsync(
            string id,
            int c1FlowRate,
            int c2FlowRate,
            int c3FlowRate,
            int c4FlowRate,
            int c5FlowRate,
            int c6FlowRate,
            int c7FlowRate
        );
        Task<Result<TrafficData>> DeleteTrafficDataAsync(string id);
        Task<Result<List<TrafficData>>> BulkDeleteTrafficDataAsync(List<ItemInfo> itemList);
        //Task<Result<TrafficData>> UpdateTrafficDataAsync(TrafficData trafficData);
    }

    public class TrafficDataService : ITrafficDataService
    {
        private readonly IDbService _dbService;
        private readonly IStoreTrafficData _storeTrafficData;

        public TrafficDataService(IDbService dbService, IStoreTrafficData storeTrafficData)
        {
            _dbService = dbService;
            _storeTrafficData = storeTrafficData;
        }

        public async Task<Result<TrafficData>> AddTrafficDataAsync(TrafficData trafficData)
        {
            try
            {
                Random rd = new Random();
                int rnum = rd.Next(1, 20000);
                trafficData.Id = rnum + DateTime.Now.ToString("HHmmssfff");
                trafficData.TrafficTime = DateTime.Now;

                var result = await _storeTrafficData.AddTrafficData(
                    _dbService.AppDbConnectionStr,
                    trafficData
                );
                return result;
            }
            catch (Exception ex)
            {
                return new Result<TrafficData>
                {
                    Status = ResultStatusValues.Error,
                    Message = ex.Message + "#" + ex.InnerException?.Message,
                };
            }
        }

        public async Task<Result<TrafficData>> GetTrafficDataAsync(string id)
        {
            try
            {
                var result = await _storeTrafficData.GetTrafficData(
                    _dbService.AppDbConnectionStr,
                    id
                );
                return result;
            }
            catch (Exception ex)
            {
                return new Result<TrafficData>
                {
                    Status = ResultStatusValues.Error,
                    Message = ex.Message + "#" + ex.InnerException?.Message
                };
            }
        }

        public async Task<Result<List<TrafficData>>> GetTrafficDataListAsync(
            TrafficDataFilter filter
        )
        {
            try
            {
                var result = await _storeTrafficData.GetTrafficDataList(
                    _dbService.AppDbConnectionStr,
                    filter
                );
                return result;
            }
            catch (Exception ex)
            {
                return new Result<List<TrafficData>>()
                {
                    Status = ResultStatusValues.Error,
                    Message = ex.Message + "#" + ex.InnerException?.Message
                };
            }
        }

        public async Task<Result<FlowRate>> UpdateFlowRateAsync(
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
            try
            {
                var result = await _storeTrafficData.UpdateFlowRate(
                    _dbService.AppDbConnectionStr,
                    id,
                    c1FlowRate,
                    c2FlowRate,
                    c3FlowRate,
                    c4FlowRate,
                    c5FlowRate,
                    c6FlowRate,
                    c7FlowRate
                );
                return result;
            }
            catch (Exception ex)
            {
                return new Result<FlowRate>
                {
                    Status = ResultStatusValues.Error,
                    Message = ex.Message + "#" + ex.InnerException?.Message
                };
            }
        }

        public async Task<Result<TrafficData>> DeleteTrafficDataAsync(string id)
        {
            try
            {
                var result = await _storeTrafficData.DeleteTrafficData(
                    _dbService.AppDbConnectionStr,
                    id
                );
                return result;
            }
            catch (Exception ex)
            {
                return new Result<TrafficData>
                {
                    Status = ResultStatusValues.Error,
                    Message = ex.Message
                };
            }
        }

        public async Task<Result<List<TrafficData>>> BulkDeleteTrafficDataAsync(
            List<ItemInfo> itemList
        )
        {
            try
            {
                var result = await _storeTrafficData.BulkDeleteTrafficData(
                    _dbService.AppDbConnectionStr,
                    itemList
                );
                return result;
            }
            catch (Exception ex)
            {
                return new Result<List<TrafficData>>
                {
                    Status = ResultStatusValues.Error,
                    Message = ex.Message + "#" + ex.InnerException?.Message
                };
            }
        }

        //public async Task<Result<TrafficData>> UpdateTrafficDataAsync(TrafficData trafficData)
        //{
        //    try
        //    {
        //        trafficData.TrafficTime = DateTime.Now;

        //        var result = await _storeTrafficData.UpdateTrafficData(_dbService.AppDbConnectionStr, trafficData);
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        return new Result<TrafficData>
        //        {
        //            Status = ResultStatusValues.Error,
        //            Message = ex.Message
        //        };
        //    }
        //}
    }
}
