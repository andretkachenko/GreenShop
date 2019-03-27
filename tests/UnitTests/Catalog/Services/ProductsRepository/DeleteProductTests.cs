using Target = GreenShop.Catalog.Services.Products.ProductsRepository;
using GreenShop.Catalog.Services.Products.Interfaces;
using Common.Interfaces;
using Common.Models.Products;
using FluentValidation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace UnitTests.Catalog.Services.ProductsRepository
{
    [TestClass]
    public class DeleteProductTests
    {
        private Mock<ISqlDataAccessor<Product>> ProductsSqlAccessorStub;
        private Mock<IMongoDataAccessor<Product>> ProductsMongoAccessorStub;
        private Mock<IProductMerger> ProductMergerStub;
        private Target ProductsRepository;

        public DeleteProductTests()
        {
            ProductsSqlAccessorStub = new Mock<ISqlDataAccessor<Product>>();
            ProductsMongoAccessorStub = new Mock<IMongoDataAccessor<Product>>();
            ProductMergerStub = new Mock<IProductMerger>();
            ProductsRepository = new Target(ProductsSqlAccessorStub.Object, ProductsMongoAccessorStub.Object, ProductMergerStub.Object);
        }

        [TestMethod]
        public void ValidId_ReturnsTrue()
        {
            // Arrange
            var id = 1;
            var mongoId = "TestMongoId";
            var expectedResult = true;

            ProductsSqlAccessorStub
                .Setup(products => products.Delete(id))
                .Returns(Task.FromResult(1));
            ProductsMongoAccessorStub
                .Setup(products => products.Delete(mongoId))
                .Returns(Task.CompletedTask);
            ProductMergerStub
                .Setup(stub => stub.GetMongoId(id))
                .Returns(mongoId);

            // Act
            var result = ProductsRepository.DeleteProduct(id);

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
            var result = ProductsRepository.GetProduct(id);

            // Assert
            Assert.AreEqual(result.Status, TaskStatus.Faulted);
            Assert.IsInstanceOfType(result.Exception.InnerException, typeof(ValidationException));
        }

        [TestMethod]
        public void InvalidId_ReturnsFalse()
        {
            // Arrange
            var id = 99999;
            var mongoId = "TestMongoId";
            var expectedResult = false;

            ProductsSqlAccessorStub
                .Setup(products => products.Delete(id))
                .Returns(Task.FromResult(0));
            ProductsMongoAccessorStub
                .Setup(products => products.Delete(mongoId))
                .Returns(Task.CompletedTask);
            ProductMergerStub
                .Setup(stub => stub.GetMongoId(id))
                .Returns(mongoId);

            // Act
            var result = ProductsRepository.DeleteProduct(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<bool>));
            Assert.AreEqual(expectedResult, result.Result);

        }
    }
}
