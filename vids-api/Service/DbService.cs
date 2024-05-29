using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vids.Configuration;
using Vids.Model;
using Microsoft.Data.SqlClient;
using Npgsql;

namespace Vids.Service
{
    public interface IDbService
    {
        public string AppDbConnectionStr { get; set; }
        public string TransDbConnectionStr { get; set; }
    }

    public partial class DbService: IDbService, IDisposable
    {
        public string AppDbConnectionStr { get; set; } = string.Empty;
        public string TransDbConnectionStr { get; set; } = string.Empty;

        private readonly IConfiguration _config;
        private readonly DbConfig _dbConfig;
        private readonly INlogger _logger;
        private string _logFolderPath = string.Empty;

        public DbService(IConfiguration config, INlogger logger)
        {
            _config = config;
            _logger = logger;

            _dbConfig = _config.GetSection("Database").Get<DbConfig>();

            var folderPathConfig = _config.GetSection("FolderPaths").Get<FolderConfig>();
            _logFolderPath = Path.Combine(folderPathConfig.LogFolder, "db-service");

            Start();
        }

        public void Dispose()
        {
            DisposeRoutine();
        }

        private void Start()
        {
            try
            {
                InitAppDb();
                InitTransDb();
                InitRoutine();

            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        private void InitAppDb()
        {
            try
            {
                foreach (var info in _dbConfig.AppDb)
                {
                    info.ConnectionStr = CreatePostgreSQLEFConnectionStr(info);
                }

                if (_dbConfig.AppDb.Count > 0)
                {
                    AppDbConnectionStr = _dbConfig.AppDb[0].ConnectionStr;
                }

                Log($"Active App Db: {AppDbConnectionStr}");
              
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        private void InitTransDb()
        {
            try
            {
                foreach (var info in _dbConfig.TransDb)
                {
                    info.ConnectionStr = CreatePostgreSQLEFConnectionStr(info);
                }

                if (_dbConfig.TransDb.Count > 0)
                {
                    TransDbConnectionStr = _dbConfig.TransDb[0].ConnectionStr;
                }

                Log($"Active Trans Db: {TransDbConnectionStr}");

            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        private static string CreatePostgreSQLEFConnectionStr(DbInfo info)
        {
            return string.Format("Host={0}; Database={1}; Port={2}; Username={3}; Password={4};", info.ServerName, info.DatabaseName, info.Port, info.Username, info.Password);
        }

        private bool TestPostgresDbConnection(string connectionStr)
        {
            if (!string.IsNullOrEmpty(connectionStr))
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionStr))
                {
                    try
                    {
                        connection.Open();
                        return true;
                    }
                    catch (Npgsql.NpgsqlException ex)
                    {
                        _logger.Error(ex);
                        return false;
                    }
                    catch (System.InvalidOperationException ex)
                    {
                        _logger.Error(ex);
                        return false;
                    }
                    catch (SqlException ex)
                    {
                        _logger.Error(ex);
                        return false;
                    }
                }
            }
            return false;
        }

        private void Log(string log)
        {
            _logger.Log(_logFolderPath, "db-service", log);
        }
    }
}