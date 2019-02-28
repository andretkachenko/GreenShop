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
    public class DeleteProductTests
    {
        private Mock<IConsumer<Product>> ProductsConsumerStub;
        private Target ProductsService;

        public DeleteProductTests()
        {
            ProductsConsumerStub = new Mock<IConsumer<Product>>();
            ProductsService = new Target(ProductsConsumerStub.Object);
        }

        [TestMethod]
        public void ValidId_ReturnsTrue()
        {
            // Arrange
            int id = 1;
            bool expectedResult = true;

            ProductsConsumerStub
                .Setup(products => products.DeleteAsync(id))
                .Returns(Task.FromResult(true));

            // Act
            Task<bool> result = ProductsService.DeleteProduct(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<bool>));
            Assert.AreEqual(expectedResult, result.Result);
        }

        [TestMethod]
        public void NegativeId_ThrowsValidationException()
        {
            // Arrange
            int id = -1;

            // Act
            Task<Product> result = ProductsService.GetProduct(id);

            // Assert
            Assert.AreEqual(result.Status, TaskStatus.Faulted);
            Assert.IsInstanceOfType(result.Exception.InnerException, typeof(ValidationException));
        }

        [TestMethod]
        public void InvalidId_ReturnsFalse()
        {
            // Arrange
            int id = 99999;
            bool expectedResult = false;

            ProductsConsumerStub
                .Setup(products => products.DeleteAsync(id))
                .Returns(Task.FromResult(false));

            // Act
            Task<bool> result = ProductsService.DeleteProduct(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<bool>));
            Assert.AreEqual(expectedResult, result.Result);

        }
    }
}
