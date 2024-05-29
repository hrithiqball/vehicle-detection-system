using NLog;

namespace Vids.Service
{
    public interface INlogger
    {
        void Debug(string log);
        void Info(string log);
        void Error(string log);
        void Error(Exception ex);
        void Warning(string log);
        void Log(string folderPath, string fileName, string log);
    }

    public class Nlogger : INlogger
    {
        private static readonly NLog.ILogger logger = LogManager.GetCurrentClassLogger();

        private ReaderWriterLockSlim ReadWriteLock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

        public void Debug(string log)
        {
            logger.Debug(log);
        }

        public void Error(string log)
        {
            logger.Error(log);
        }

        public void Error(Exception ex)
        {
            logger.Error(ex);
        }

        public void Info(string log)
        {
            logger.Info(log);
        }

        public void Warning(string log)
        {
            logger.Warn(log);
        }

        public void Log(string folderPath, string fileName, string log)
        {
            logger.Debug(log);

            Directory.CreateDirectory(folderPath);

            if (Directory.Exists(folderPath))
            {
                ReadWriteLock.EnterWriteLock();

                try
                {
                    string filePath = Path.Combine(folderPath, fileName + "-" + DateTime.Today.ToString("yyMMdd") + ".log");

                    using (StreamWriter streamWriter = new StreamWriter(filePath, true))
                    {
                        if (string.IsNullOrEmpty(log))
                        {
                            log = "\r\n";
                        }
                        else
                        {
                            log = string.Format("{0}| {1}", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), log);
                        }
                        streamWriter.WriteLine(log);
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                }
                finally
                {
                    if (ReadWriteLock.IsWriteLockHeld)
                    {
                        ReadWriteLock.ExitWriteLock();
                    }
                }
            }
        }

    }
}
