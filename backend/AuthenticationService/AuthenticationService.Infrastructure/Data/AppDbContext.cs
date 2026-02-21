using AuthenticationService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationService.Infrastructure.Data
{
    // Entity Framework database context for the application
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Database table for users
        public DbSet<User> Users { get; set; }

        // Configure entity mappings
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User table configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PasswordHash).IsRequired();
                entity.HasIndex(e => e.Email).IsUnique();
            });
        }
    }
}