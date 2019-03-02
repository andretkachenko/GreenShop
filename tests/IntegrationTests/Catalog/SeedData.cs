using Common.Models.Products;
using System;

namespace IntegrationTests.Catalog
{
    public static class SeedData
    {
        public static void PopulateTestData(AppDbContext dbContext)
        {
            dbContext.Products.Add(new Product
            {
                Id = 1,
                MongoId = Guid.NewGuid().ToString(),
                BasePrice = 10,
                CategoryId = 1,
                Description = "First Integration Product Description",
                Name = "First Integration Product Name",
                Rating = 5,
            });
            dbContext.Products.Add(new Product
            {
                Id = 2,
                MongoId = Guid.NewGuid().ToString(),
                BasePrice = 8,
                CategoryId = 2,
                Description = "Second Integration Product Description",
                Name = "Second Integration Product Name",
                Rating = 4,
            });
            dbContext.SaveChanges();
        }
    }
}
