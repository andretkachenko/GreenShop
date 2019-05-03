using GreenShop.Web.Bff.Shopping.Api.Models.Categories;
using GreenShop.Web.Bff.Shopping.Api.Models.Products;
using GreenShop.Web.Bff.Shopping.Api.Services.Catalog.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using Target = GreenShop.Web.Bff.Shopping.Api.Services.CatalogService;

namespace GreenShop.Web.Bff.Shopping.UnitTests.Services.CatalogService
{
    [TestClass]
    public class EditProductAsyncTests
    {
        private Mock<IConsumer<Category>> CategoriesConsumerStub;
        private Mock<IConsumer<Product>> ProductsConsumerStub;
        private Mock<ICommentsConsumer> CommentsConsumerStub;
        private Target CatalogService;

        public EditProductAsyncTests()
        {
            CategoriesConsumerStub = new Mock<IConsumer<Category>>();
            ProductsConsumerStub = new Mock<IConsumer<Product>>();
            CommentsConsumerStub = new Mock<ICommentsConsumer>();
            CatalogService = new Target(CategoriesConsumerStub.Object, ProductsConsumerStub.Object, CommentsConsumerStub.Object);
        }

        [TestMethod]
        public void ValidProduct_ReturnsTrue()
        {
            // Arrange
            int id = 1;
            string name = "RenamedTestProduct";
            int parentId = 3;
            string description = "TestDescription";
            decimal basePrice = 12m;
            float rating = 4.5f;
            bool expectedResult = true;

            Product product = new Product
            {
                Id = id,
                Name = name,
                CategoryId = parentId,
                Description = description,
                BasePrice = basePrice,
                Rating = rating
            };

            ProductsConsumerStub
                .Setup(products => products.EditAsync(product))
                .Returns(Task.FromResult(true));

            // Act
            Task<bool> result = CatalogService.EditProductAsync(product);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<bool>));
            Assert.AreEqual(expectedResult, result.Result);
        }

        [TestMethod]
        public void InvalidProductId_ReturnsFalse()
        {
            // Arrange
            int id = 99999;
            string name = "NonExistingProduct";
            int parentId = 3;
            string description = "TestDescription";
            decimal basePrice = 12m;
            float rating = 4.5f;
            bool expectedResult = false;

            Product product = new Product
            {
                Id = id,
                Name = name,
                CategoryId = parentId,
                Description = description,
                BasePrice = basePrice,
                Rating = rating
            };

            ProductsConsumerStub
                .Setup(products => products.EditAsync(product))
                .Returns(Task.FromResult(false));

            // Act
            Task<bool> result = CatalogService.EditProductAsync(product);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<bool>));
            Assert.AreEqual(expectedResult, result.Result);
        }
    }
}
