using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TESTCRUDNET6.Data
{
    public class MyDbContext : IdentityDbContext<ApplicationUser>
    {
        /* public MyDbContext(DbContextOptions options) : base(options)
         {

         }*/

        public MyDbContext(DbContextOptions<MyDbContext> opt) : base(opt)
        {

        }

        #region DbSet
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");
                entity.HasKey(e => e.ProductId);
                entity.Property(e => e.ProductName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Price).IsRequired();
                entity.Property(e => e.Image).IsRequired();
                entity.HasOne(e => e.Category).WithMany(e => e.Products);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("category");
                entity.HasKey(e => e.CategoryId);
                entity.Property(e => e.CategoryName).IsRequired().HasMaxLength(50);
                entity.HasMany(e => e.Products).WithOne(e => e.Category);
            });

        }
    }
}
