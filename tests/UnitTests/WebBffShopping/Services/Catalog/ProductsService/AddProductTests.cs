using Common.Models.Products;
using FluentValidation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using Web.Bff.Shopping.Services.Catalog.Interfaces;
using Target = Web.Bff.Shopping.Services.Catalog.ProductsService;

namespace UnitTests.WebBffShopping.Services.Catalog.ProductsService
{
    [TestClass]
    public class AddProductTests
    {
        private Mock<IConsumer<Product>> ProductsConsumerStub;
        private Target ProductsService;

        public AddProductTests()
        {
            ProductsConsumerStub = new Mock<IConsumer<Product>>();
            ProductsService = new Target(ProductsConsumerStub.Object);
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

            ProductsConsumerStub
                .Setup(products => products.AddAsync(It.IsAny<Product>()))
                .Returns(Task.FromResult(expectedId));

            // Act
            Task<int> result = ProductsService.AddProduct(product);

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
            Task<int> result = ProductsService.AddProduct(product);

            // Assert
            Assert.AreEqual(result.Status, TaskStatus.Faulted);
            Assert.IsInstanceOfType(result.Exception.InnerException, typeof(ValidationException));
        }
    }
}
