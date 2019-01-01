using Catalog.Services.Products;
using Common.Interfaces;
using Common.Models.Products;
using FluentValidation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace UnitTests.CatalogTests.ServiceTests.ProductsRepositoryTests
{
    [TestClass]
    public class DeleteProductTests
    {
        private Mock<IDataAccessor<Product>> ProductsAccessorMock;
        private ProductsRepository Service;

        public DeleteProductTests()
        {
            ProductsAccessorMock = new Mock<IDataAccessor<Product>>();
            Service = new ProductsRepository(ProductsAccessorMock.Object);
        }

        [TestMethod]
        public void ValidId_ReturnsTrue()
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
        public void NegativeId_ThrowsValidationException()
        {
            // Arrange
            var id = -1;

            // Act
            var result = Service.GetProduct(id);

            // Assert
            Assert.AreEqual(result.Status, TaskStatus.Faulted);
            Assert.IsInstanceOfType(result.Exception.InnerException, typeof(ValidationException));
        }

        [TestMethod]
        public void InvalidId_ReturnsFalse()
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
