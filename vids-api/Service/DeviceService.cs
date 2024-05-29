using Vids.Database;
using Vids.Model;

namespace Vids.Service
{
    public interface IDeviceService
    {
        Task<Result<Device>> AddDeviceAsync(Device device);
        Task<Result<Device>> GetDeviceAsync(string deviceId);
        Task<Result<List<Device>>> GetDeviceListAsync(DeviceFilter filter);
        Task<Result<LaneName>> UpdateLaneNameAsync(
            string deviceId,
            string lane1Name,
            string lane2Name,
            string lane3Name,
            string lane4Name,
            string lane5Name,
            string lane6Name
        );
        Task<Result<ClassName>> UpdateClassNameAsync(
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

    public class DeviceService : IDeviceService
    {
        private readonly IDbService _dbService;
        private readonly IStoreDevice _storeDevice;

        public DeviceService(IStoreDevice storeDevice, IDbService dbService)
        {
            _storeDevice = storeDevice;
            _dbService = dbService;
        }

        public async Task<Result<Device>> AddDeviceAsync(Device device)
        {
            try
            {
                Random rd = new Random();
                int rnum = rd.Next(1, 200);
                device.CreatedTime = DateTime.Now;
                device.DeviceId = rnum + DateTime.Now.ToString("HHmmss");

                var result = await _storeDevice.AddDevice(_dbService.AppDbConnectionStr, device);
                return result;
            }
            catch (Exception ex)
            {
                return new Result<Device>
                {
                    Status = ResultStatusValues.Error,
                    Message = ex.Message + "#" + ex.InnerException?.Message,
                };
            }
        }

        public async Task<Result<Device>> GetDeviceAsync(string deviceId)
        {
            try
            {
                var result = await _storeDevice.GetDevice(_dbService.AppDbConnectionStr, deviceId);
                return result;
            }
            catch (Exception ex)
            {
                return new Result<Device>
                {
                    Status = ResultStatusValues.Error,
                    Message = ex.Message + "#" + ex.InnerException?.Message
                };
            }
        }

        public async Task<Result<List<Device>>> GetDeviceListAsync(DeviceFilter filter)
        {
            try
            {
                var result = await _storeDevice.GetDeviceList(
                    _dbService.AppDbConnectionStr,
                    filter
                );
                return result;
            }
            catch (Exception ex)
            {
                return new Result<List<Device>>()
                {
                    Status = ResultStatusValues.Error,
                    Message = ex.Message + "#" + ex.InnerException?.Message
                };
            }
        }

        public async Task<Result<LaneName>> UpdateLaneNameAsync(
            string deviceId,
            string lane1Name,
            string lane2Name,
            string lane3Name,
            string lane4Name,
            string lane5Name,
            string lane6Name
        )
        {
            try
            {
                var result = await _storeDevice.UpdateLaneName(
                    _dbService.AppDbConnectionStr,
                    deviceId,
                    lane1Name,
                    lane2Name,
                    lane3Name,
                    lane4Name,
                    lane5Name,
                    lane6Name
                );
                return result;
            }
            catch (Exception ex)
            {
                return new Result<LaneName>
                {
                    Status = ResultStatusValues.Error,
                    Message = ex.Message + "#" + ex.InnerException?.Message
                };
            }
        }

        public async Task<Result<ClassName>> UpdateClassNameAsync(
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
            try
            {
                var result = await _storeDevice.UpdateClassName(
                    _dbService.AppDbConnectionStr,
                    deviceId,
                    c1Name,
                    c2Name,
                    c3Name,
                    c4Name,
                    c5Name,
                    c6Name,
                    c7Name
                );
                return result;
            }
            catch (Exception ex)
            {
                return new Result<ClassName>
                {
                    Status = ResultStatusValues.Error,
                    Message = ex.Message + "#" + ex.InnerException?.Message
                };
            }
        }
    }
}
