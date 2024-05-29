using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vids.Service
{
    public partial class DbService
    {
        private CancellationTokenSource? _routineTokenSource = null;

        private void InitRoutine()
        {
            try
            {
                _routineTokenSource = new CancellationTokenSource();

                Task.Run(async () =>
                {
                    await RunAppDbRoutine();
                });

                Task.Run(async () =>
                {
                    await RunTransDbRoutine();
                });


            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        private void DisposeRoutine()
        {
            try
            {
                if(_routineTokenSource != null)
                {
                    _routineTokenSource.Cancel();
                    _routineTokenSource = null; 
                }
                
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        private async Task RunAppDbRoutine()
        {
            try
            {
                while (_routineTokenSource!=null && !_routineTokenSource.IsCancellationRequested)
                {                  
                    CheckAppDb();

                    await Task.Delay(_dbConfig.RoutineIntervalS * 1000);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        private async Task RunTransDbRoutine()
        {
            try
            {
                while (_routineTokenSource!=null && !_routineTokenSource.IsCancellationRequested)
                {                  
                    CheckTransDb();

                    await Task.Delay(_dbConfig.RoutineIntervalS * 1000);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        private void CheckAppDb()
        {
            try
            {
                List<Task> taskList = new List<Task>();

                foreach (var item in _dbConfig.AppDb)
                {
                    var tmpTask = Task.Run(() => { item.IsOnline = TestPostgresDbConnection(item.ConnectionStr); });
                    taskList.Add(tmpTask);
                }

                Task.WaitAll(taskList.ToArray());

                var currentDbConnectionStr = string.Empty;
                foreach (var item in _dbConfig.AppDb)
                {
                    if (item.IsOnline)
                    {
                        currentDbConnectionStr = item.ConnectionStr;
                        break;
                    }
                }

                if (AppDbConnectionStr != currentDbConnectionStr)
                {
                    Log($"Active App Db: {AppDbConnectionStr}");
                }

                AppDbConnectionStr = currentDbConnectionStr;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }
    
        private void CheckTransDb()
        {
            try
            {
                List<Task> taskList = new List<Task>();

                foreach (var item in _dbConfig.TransDb)
                {
                    var tmpTask = Task.Run(() => { item.IsOnline = TestPostgresDbConnection(item.ConnectionStr); });
                    taskList.Add(tmpTask);
                }

                Task.WaitAll(taskList.ToArray());

                var currentDbConnectionStr = string.Empty;
                foreach (var item in _dbConfig.TransDb)
                {
                    if (item.IsOnline)
                    {
                        currentDbConnectionStr = item.ConnectionStr;
                        break;
                    }
                }

                if (TransDbConnectionStr != currentDbConnectionStr)
                {
                    Log($"Active Trans Db: {TransDbConnectionStr}");
                }

                TransDbConnectionStr = currentDbConnectionStr;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }
    }
}