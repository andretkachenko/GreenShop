using Common.Models.Categories;
using Common.Models.Products;
using FluentValidation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using Web.Bff.Shopping.Services.Catalog.Interfaces;
using Target = Web.Bff.Shopping.Services.CatalogService;

namespace UnitTests.WebBffShopping.Services.CatalogService
{
    [TestClass]
    public class AddProductAsyncTests
    {
        private Mock<IConsumer<Category>> CategoriesConsumerStub;
        private Mock<IConsumer<Product>> ProductsConsumerStub;
        private Mock<ICommentsConsumer> CommentsConsumerStub;
        private Target CatalogService;

        public AddProductAsyncTests()
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
