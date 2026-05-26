using IdopontfoglaloWebalk.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace IdopontfoglaloWebalk.Context
{
    public class EfContext : IdentityDbContext<Users>
    {
        public EfContext(DbContextOptions<EfContext> options)
            : base(options)
        {
        }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<Services> Services { get; set; }
        public DbSet<ServiceCategories> ServiceCategories { get; set; }
        public DbSet<Occasions> Occasions { get; set; }
        public DbSet<Feedbacks> Feedbacks { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Categories>().HasKey(c => c.category_id);
            builder.Entity<Services>().HasKey(s => s.service_id);
            builder.Entity<Occasions>().HasKey(o => o.reservation_id);
            builder.Entity<Feedbacks>().HasKey(f => f.feedback_id);
            builder.Entity<ServiceCategories>().HasKey(sc => sc.Id);

            builder.Entity<Services>()
                .HasOne(s => s.Category)
                .WithMany(c => c.Services)
                .HasForeignKey(s => s.category_id)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Services>()
                .HasOne(s => s.Owner)
                .WithMany()
                .HasForeignKey(s => s.owner_id)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ServiceCategories>()
                .HasOne(sc => sc.Service)
                .WithMany(s => s.ServiceCategories)
                .HasForeignKey(sc => sc.service_id)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Occasions>()
                .HasOne(o => o.ServiceCategory)
                .WithMany(sc => sc.Occasions)
                .HasForeignKey(o => o.service_category_id)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Occasions>()
                .HasOne(o => o.User)
                .WithMany()
                .HasForeignKey(o => o.user_id)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Feedbacks>()
                .HasOne(f => f.Reservation)
                .WithMany()
                .HasForeignKey(f => f.reservation_Id)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Feedbacks>()
                .HasOne(f => f.ServiceOwner)
                .WithMany()
                .HasForeignKey(f => f.service_owner_id)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Feedbacks>()
                .HasOne(f => f.Guest)
                .WithMany()
                .HasForeignKey(f => f.guest_id)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}