using Target = Catalog.Services.Products.ProductsRepository;
using Catalog.Services.Products.Interfaces;
using Common.Interfaces;
using Common.Models.Products;
using FluentValidation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace UnitTests.Catalog.Services.ProductsRepository
{
    [TestClass]
    public class AddProductTests
    {
        private Mock<ISqlDataAccessor<Product>> ProductsSqlAccessorStub;
        private Mock<IMongoDataAccessor<Product>> ProductsMongoAccessorStub;
        private Mock<IProductMerger> ProductMergerStub;
        private Target ProductsRepository;

        public AddProductTests()
        {
            ProductsSqlAccessorStub = new Mock<ISqlDataAccessor<Product>>();
            ProductsMongoAccessorStub = new Mock<IMongoDataAccessor<Product>>();
            ProductMergerStub = new Mock<IProductMerger>();
            ProductsRepository = new Target(ProductsSqlAccessorStub.Object, ProductsMongoAccessorStub.Object, ProductMergerStub.Object);
        }

        [TestMethod]
        public void ValidProduct_ReturnsTrue()
        {
            // Arrange
            var id = 1;
            var expectedResult = true;

            ProductsSqlAccessorStub
                .Setup(products => products.Delete(id))
                .Returns(Task.FromResult(1));

            // Act
            var result = ProductsRepository.DeleteProduct(id);

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
            var result = ProductsRepository.DeleteProduct(id);

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

            ProductsSqlAccessorStub
                .Setup(products => products.Delete(id))
                .Returns(Task.FromResult(0));

            // Act
            var result = ProductsRepository.DeleteProduct(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<bool>));
            Assert.AreEqual(expectedResult, result.Result);
        }
    }
}
