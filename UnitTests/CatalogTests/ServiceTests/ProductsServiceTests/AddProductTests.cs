using Catalog.Services.Products;
using Common.Interfaces;
using Common.Models.Products;
using FluentValidation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace UnitTests.CatalogTests.ServiceTests.ProductsServiceTests
{
    [TestClass]
    public class AddProductTests
    {
        private Mock<IDataAccessor<Product>> ProductsAccessorMock;
        private ProductsService Service;

        public AddProductTests()
        {
            ProductsAccessorMock = new Mock<IDataAccessor<Product>>();
            Service = new ProductsService(ProductsAccessorMock.Object);
        }

        [TestMethod]
        public void ValidProduct_ReturnsTrue()
        {
            // Arrange
            var id = 1;
            var expectedResult = true;

            ProductsAccessorMock
                .Setup(products => products.Delete(id))
                .Returns(Task.FromResult(1));

            // Act
            var result = Service.DeleteProduct(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<bool>));
            Assert.AreEqual(expectedResult, result.Result);
        }

        [TestMethod]
        public void NegativeProductId_ThrowsValidationException()
        {
            // Arrange
            var id = -1;

            // Act
            var result = Service.DeleteProduct(id);

            // Assert
            Assert.AreEqual(result.Status, TaskStatus.Faulted);
            Assert.IsInstanceOfType(result.Exception.InnerException, typeof(ValidationException));
        }

        [TestMethod]
        public void InvalidProductId_ReturnsFalse()
        {
            // Arrange
            var id = 99999;
            var expectedResult = false;

            ProductsAccessorMock
                .Setup(products => products.Delete(id))
                .Returns(Task.FromResult(0));

            // Act
            var result = Service.DeleteProduct(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<bool>));
            Assert.AreEqual(expectedResult, result.Result);
        }
    }
}
