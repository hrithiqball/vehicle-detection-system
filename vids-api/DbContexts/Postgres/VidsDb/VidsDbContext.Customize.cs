using Microsoft.EntityFrameworkCore;

namespace Vids.DbContexts.Postgres.VidsDb
{
    public partial class VidsDbContext
    {
        private string _connectionStr = string.Empty; //"Server=172.17.0.35\sqlexpress;Database=TmcsConfig;User ID=admin;Password=admin"

        public VidsDbContext(string connectionStr)
        {
            _connectionStr = connectionStr;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseLazyLoadingProxies().UseNpgsql(_connectionStr);
            }
        }
    }
}
