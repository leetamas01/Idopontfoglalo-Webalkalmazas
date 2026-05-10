using IdopontfoglaloWebalk.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdopontfoglaloWebalk.Context
{
    public class EfContext : IdentityDbContext<User>
    {
        public EfContext(DbContextOptions<EfContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

        }
    }
}