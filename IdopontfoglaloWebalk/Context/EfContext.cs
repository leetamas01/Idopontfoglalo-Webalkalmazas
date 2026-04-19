using IdopontfoglaloWebalk.Models;
using Microsoft.EntityFrameworkCore;

namespace IdopontfoglaloWebalk.Context
{
    public class EfContext: DbContext
    {
        public DbSet<User> Users { get; set; }

        public EfContext(DbContextOptions<EfContext> options)
        : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = "server=127.0.0.1;port=3306;database=idopontfoglalo_db;uid=root;pwd=;allowPublicKeyRetrieval=true;sslMode=none";
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }
    }
}
