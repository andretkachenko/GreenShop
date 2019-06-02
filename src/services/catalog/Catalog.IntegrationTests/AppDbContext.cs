using GreenShop.Catalog.IntegrationTests.Wrappers;
using Microsoft.EntityFrameworkCore;

namespace GreenShop.Catalog.IntegrationTests
{
    public class AppDbContext : DbContext
    {
        public AppDbContext()
        {

        }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=products.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductWrapper>().Ignore(x => x.Comments);
            modelBuilder.Entity<ProductWrapper>().Ignore(x => x.Specifications);

            /* restore me to have data in db
             modelBuilder.Entity<Product>().HasData(new Product
            {
                Id = 1,
                MongoId = Guid.NewGuid().ToString(),
                BasePrice = 10,
                CategoryId = 1,
                Description = "First Integration Product Description",
                Name = "First Integration Product Name",
                Rating = 5,
            });
             modelBuilder.Entity<Product>().HasData(new Product
            {
                Id = 2,
                MongoId = Guid.NewGuid().ToString(),
                BasePrice = 8,
                CategoryId = 2,
                Description = "Second Integration Product Description",
                Name = "Second Integration Product Name",
                Rating = 4,
            });
             */
        }

        internal DbSet<ProductWrapper> Products { get; set; }
    }
}
