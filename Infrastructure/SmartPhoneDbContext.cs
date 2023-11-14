using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class SmartPhoneDbContext : DbContext
    {
        public SmartPhoneDbContext(DbContextOptions<SmartPhoneDbContext> options)
            : base(options)
        {
        }
            
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SmartPhoneDbContext).Assembly);
        }
        public DbSet<Product> Product { get; set; }

        public DbSet<Capacity> Capacity { get; set; }

        public DbSet<Category> Category { get; set; }

        public DbSet<Color> Color { get; set; }

    }
}
