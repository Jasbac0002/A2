using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NZWSO.Models;

namespace NZWSO.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Product>()
                .HasMany(x => x.Users)
                .WithMany(x => x.Products)
                .UsingEntity<UserProducts>()
                .HasKey(d => new { d.UserId, d.ProductId });
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<UserProducts> UserProducts { get; set; }
    }
}