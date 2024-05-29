using Vids.Database;
using Vids.Model;
using Vids.Service;

namespace Vids.Service
{
    public interface IConfigService
    {
        Task<Result<T>> GetConfigAsync<T>(string ownerId, string configName);
        Task<Result<T>> UpdateConfigAsync<T>(string ownerId, string configName, T config);
    }

    public class ConfigService : IConfigService
    {
        private readonly INlogger _logger;
        private readonly IDbService _dbService;
        private readonly IStoreConfig _storeConfig;

        public ConfigService(INlogger logger, IDbService dbService, IStoreConfig storeConfig)
        {
            _logger = logger;
            _dbService = dbService;
            _storeConfig = storeConfig;
        }

        public async Task<Result<T>> GetConfigAsync<T>(string ownerId, string configName)
        {
            try
            {
                var result = await _storeConfig.GetConfigAsync<T>(_dbService.AppDbConnectionStr,ownerId, configName);               
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return new Result<T>
                {
                    Status = ResultStatusValues.Error,
                    Message = ex.Message + "#" + ex.InnerException?.Message
                };
            }
        }

        public async Task<Result<T>> UpdateConfigAsync<T>(string ownerId, string configName, T config)
        {
            try
            {
                var result = await _storeConfig.SaveConfigAsync<T>(_dbService.AppDbConnectionStr, ownerId, configName, config);
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return new Result<T>
                {
                    Status = ResultStatusValues.Error,
                    Message = ex.Message + "#" + ex.InnerException?.Message
                };
            }
        }
    }
}
