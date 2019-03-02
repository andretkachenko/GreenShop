using Common.Models.Categories;
using Common.Models.Products;
using Microsoft.EntityFrameworkCore;

namespace IntegrationTests.Catalog
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
            modelBuilder.Entity<Product>().Ignore(x => x.Category);
            modelBuilder.Entity<Product>().Ignore(x => x.Comments);
            modelBuilder.Entity<Product>().Ignore(x => x.Specifications);

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

        public DbSet<Product> Products { get; set; }
    }
}
