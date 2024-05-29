using Vids.Database;
using Vids.DbContexts.Postgres.VidsDb;
using Vids.Model;

namespace Vids.Service
{
    public interface IVehicleService
    {
        Task<Result<Vehicle>> AddVehicleAsync(Vehicle vehicle);
        Task<Result<Vehicle>> GetVehicleAsync(string id);
        Task<Result<List<Vehicle>>> GetVehicleListAsync(VehicleFilter filter);
        Task<Result<Vehicle>> UpdateVehicleAsync(Vehicle vehicle);
        Task<Result<Vehicle>> DeleteVehicleAsync(string id);
    }

    public class VehicleService : IVehicleService
    {
        private readonly IDbService _dbService;
        private readonly IStoreVehicle _storeVehicle;

        public VehicleService(IDbService dbService, IStoreVehicle storeVehicle)
        {
            _dbService = dbService;
            _storeVehicle = storeVehicle;
        }

        public async Task<Result<Vehicle>> AddVehicleAsync(Vehicle vehicle)
        {
            try
            {
                Random rd = new Random();
                int rnum = rd.Next(1, 20000);
                vehicle.Id = rnum + DateTime.Now.ToString("HHmmssfff");
                vehicle.PassingTime = DateTime.Now;

                var result = await _storeVehicle.AddVehicle(_dbService.AppDbConnectionStr, vehicle);
                return result;
            }
            catch (Exception ex)
            {
                return new Result<Vehicle>
                {
                    Status = ResultStatusValues.Error,
                    Message = ex.Message + "#" + ex.InnerException?.Message,
                };
            }
        }

        public async Task<Result<Vehicle>> GetVehicleAsync(string id)
        {
            try
            {
                var result = await _storeVehicle.GetVehicle(_dbService.AppDbConnectionStr, id);
                return result;
            }
            catch (Exception ex)
            {
                return new Result<Vehicle>
                {
                    Status = ResultStatusValues.Error,
                    Message = ex.Message + "#" + ex.InnerException?.Message
                };
            }
        }

        public async Task<Result<List<Vehicle>>> GetVehicleListAsync(VehicleFilter filter)
        {
            try
            {
                var result = await _storeVehicle.GetVehicleList(
                    _dbService.AppDbConnectionStr,
                    filter
                );
                return result;
            }
            catch (Exception ex)
            {
                return new Result<List<Vehicle>>()
                {
                    Status = ResultStatusValues.Error,
                    Message = ex.Message + "#" + ex.InnerException?.Message
                };
            }
        }

        public async Task<Result<Vehicle>> UpdateVehicleAsync(Vehicle vehicle)
        {
            try
            {
                vehicle.PassingTime = DateTime.Now;

                var result = await _storeVehicle.UpdateVehicle(
                    _dbService.AppDbConnectionStr,
                    vehicle
                );
                return result;
            }
            catch (Exception ex)
            {
                return new Result<Vehicle>
                {
                    Status = ResultStatusValues.Error,
                    Message = ex.Message + "#" + ex.InnerException?.Message
                };
            }
        }

        public async Task<Result<Vehicle>> DeleteVehicleAsync(string id)
        {
            try
            {
                var result = await _storeVehicle.DeleteVehicle(_dbService.AppDbConnectionStr, id);
                return result;
            }
            catch (Exception ex)
            {
                return new Result<Vehicle>
                {
                    Status = ResultStatusValues.Error,
                    Message = ex.Message
                };
            }
        }
    }
}
