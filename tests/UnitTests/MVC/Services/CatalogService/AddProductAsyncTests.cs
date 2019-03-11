using Common.Models.Products;
using FluentValidation;
using GreenShop.MVC.Services.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using Target = GreenShop.MVC.Services.CatalogService;

namespace UnitTests.MVC.Services.CatalogService
{
    [TestClass]
    public class AddProductAsyncTests
    {
        private Mock<ICatalogConsumer> CatalogConsumerStub;
        private Target CatalogService;

        public AddProductAsyncTests()
        {
            CatalogConsumerStub = new Mock<ICatalogConsumer>();
            CatalogService = new Target(CatalogConsumerStub.Object);
        }

        [TestMethod]
        public void ValidProduct_ReturnsTrue()
        {
            // Arrange
            int expectedId = 1;
            string name = "RenamedTestProduct";
            int parentId = 3;
            string description = "TestDescription";
            decimal basePrice = 12m;
            float rating = 4.5f;

            Product product = new Product
            {
                Name = name,
                CategoryId = parentId,
                Description = description,
                BasePrice = basePrice,
                Rating = rating
            };

            CatalogConsumerStub
                .Setup(catalog => catalog.AddProductAsync(It.IsAny<Product>()))
                .Returns(Task.FromResult(expectedId));

            // Act
            Task<int> result = CatalogService.AddProductAsync(product);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<int>));
            Assert.AreEqual(expectedId, result.Result);
        }

        [TestMethod]
        public void EmptyName_ThrowsValidationException()
        {
            // Arrange
            string name = "";

            Product product = new Product
            {
                Name = name
            };

            // Act
            Task<int> result = CatalogService.AddProductAsync(product);

            // Assert
            Assert.AreEqual(result.Status, TaskStatus.Faulted);
            Assert.IsInstanceOfType(result.Exception.InnerException, typeof(ValidationException));
        }
    }
}
