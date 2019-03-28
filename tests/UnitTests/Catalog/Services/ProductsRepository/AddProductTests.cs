using FluentValidation;
using GreenShop.Catalog.DataAccessors.Interfaces;
using GreenShop.Catalog.Models.Products;
using GreenShop.Catalog.Services.Products.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using Target = GreenShop.Catalog.Services.Products.ProductsRepository;

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

            ProductsSqlAccessorStub
                .Setup(products => products.Add(It.IsAny<Product>()))
                .Returns(Task.FromResult(expectedId));
            ProductsMongoAccessorStub
                .Setup(products => products.Add(It.IsAny<Product>()))
                .Returns(Task.CompletedTask);

            // Act
            Task<int> result = ProductsRepository.AddProduct(product);

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
            Task<int> result = ProductsRepository.AddProduct(product);

            // Assert
            Assert.AreEqual(result.Status, TaskStatus.Faulted);
            Assert.IsInstanceOfType(result.Exception.InnerException, typeof(ValidationException));
        }
    }
}
