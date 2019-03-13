using Common.Models.Categories;
using FluentValidation;
using GreenShop.MVC.Services.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using Target = GreenShop.MVC.Services.CatalogService;

namespace UnitTests.MVC.Services.CatalogService
{
    [TestClass]
    public class AddCategoryAsyncTests
    {
        private Mock<ICatalogConsumer> CatalogConsumerStub;
        private Target CatalogService;

        public AddCategoryAsyncTests()
        {
            CatalogConsumerStub = new Mock<ICatalogConsumer>();
            CatalogService = new Target(CatalogConsumerStub.Object);
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

            CatalogConsumerStub
                .Setup(catalog => catalog.AddCategoryAsync(expectedCategory))
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
