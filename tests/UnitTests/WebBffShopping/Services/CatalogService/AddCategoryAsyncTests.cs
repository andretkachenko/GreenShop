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
    public class AddCategoryAsyncTests
    {
        private Mock<IConsumer<Category>> CategoriesConsumerStub;
        private Mock<IConsumer<Product>> ProductsConsumerStub;
        private Target CatalogService;

        public AddCategoryAsyncTests()
        {
            CategoriesConsumerStub = new Mock<IConsumer<Category>>();
            ProductsConsumerStub = new Mock<IConsumer<Product>>();
            CatalogService = new Target(CategoriesConsumerStub.Object, ProductsConsumerStub.Object);
        }

        [TestMethod]
        public void ValidCategory_ReturnsExpectedId()
        {
            // Arrange
            int id = 1;
            string name = "TestName";
            int parentCategoryId = 2;

            Category expectedCategory = new Category
            {
                Id = id,
                Name = name,
                ParentCategoryId = parentCategoryId
            };

            CategoriesConsumerStub
                .Setup(categories => categories.AddAsync(expectedCategory))
                .Returns(Task.FromResult(id));

            // Act
            Task<int> result = CatalogService.AddCategoryAsync(expectedCategory);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<int>));
            Assert.AreEqual(id, result.Result);
        }

        [TestMethod]
        public void EmptyName_ThrowsValidationException()
        {
            // Arrange
            string invalidName = string.Empty;
            Category invalidCategory = new Category
            {
                Name = invalidName
            };

            // Act
            Task<int> result = CatalogService.AddCategoryAsync(invalidCategory);

            // Assert
            Assert.AreEqual(result.Status, TaskStatus.Faulted);
            Assert.IsInstanceOfType(result.Exception.InnerException, typeof(ValidationException));
        }
    }
}
