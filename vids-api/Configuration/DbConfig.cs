using Vids.Model;

namespace Vids.Configuration
{
    public class DbConfig
    {
        public int RoutineIntervalS { get; set; } = 10;
        public List<DbInfo> AppDb { get; set; } = new List<DbInfo>();
        public List<DbInfo> TransDb { get; set; } = new List<DbInfo>();
    }
}
