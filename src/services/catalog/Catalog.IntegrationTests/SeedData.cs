using GreenShop.Catalog.IntegrationTests.Wrappers;

namespace GreenShop.Catalog.IntegrationTests
{
    public static class SeedData
    {
        public static void PopulateTestData(AppDbContext dbContext)
        {
            dbContext.Products.Add(new ProductWrapper
            {
                WrapId = 1,
                WrapBasePrice = 10,
                WrapCategoryId = 1,
                WrapDescription = "First Integration Product Description",
                WrapName = "First Integration Product Name",
                WrapRating = 5,
            });
            dbContext.Products.Add(new ProductWrapper
            {
                WrapId = 2,
                WrapBasePrice = 8,
                WrapCategoryId = 2,
                WrapDescription = "Second Integration Product Description",
                WrapName = "Second Integration Product Name",
                WrapRating = 4,
            });
            dbContext.SaveChanges();
        }
    }
}
