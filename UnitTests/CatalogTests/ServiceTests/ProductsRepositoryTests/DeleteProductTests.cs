using Catalog.Services.Products;
using Catalog.Services.Products.Interfaces;
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
        private Mock<ISqlDataAccessor<Product>> ProductsSqlAccessorMock;
        private Mock<IMongoDataAccessor<Product>> ProductsMongoAccessorMock;
        private Mock<IProductMerger> ProductMergerMock;
        private ProductsRepository Service;

        public DeleteProductTests()
        {
            ProductsSqlAccessorMock = new Mock<ISqlDataAccessor<Product>>();
            ProductsMongoAccessorMock = new Mock<IMongoDataAccessor<Product>>();
            ProductMergerMock = new Mock<IProductMerger>();
            Service = new ProductsRepository(ProductsSqlAccessorMock.Object, ProductsMongoAccessorMock.Object, ProductMergerMock.Object);
        }

        [TestMethod]
        public void ValidId_ReturnsTrue()
        {
            // Arrange
            var id = 1;
            var expectedResult = true;

            ProductsSqlAccessorMock
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

            ProductsSqlAccessorMock
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
