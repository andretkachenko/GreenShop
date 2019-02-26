using Target = Web.Bff.Shopping.Services.Catalog.CategoriesService;
using Common.Interfaces;
using Common.Models.Categories;
using FluentValidation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using Web.Bff.Shopping.Services.Catalog.Consumers;
using Web.Bff.Shopping.Config;
using Microsoft.Extensions.Options;
using Web.Bff.Shopping.Services.Catalog.Interfaces;

namespace UnitTests.WebBffShopping.Services.Catalog.CategoriesService
{
    [TestClass]
    public class EditCategoryTests
    {
        private Mock<IConsumer<Category>> CategoriesConsumerStub;
        private Target Service;

        public EditCategoryTests()
        {
            CategoriesConsumerStub = new Mock<IConsumer<Category>>();
            Service = new Target(CategoriesConsumerStub.Object);
        }

        [TestMethod]
        public void ValidCategory_ReturnsTrue()
        {
            // Arrange
            var id = 1;
            var name = "RenamedTestCategory";
            var parentId = 3;
            var expectedResult = true;

            var category = new Category
            {
                Id = id,
                Name = name,
                ParentCategoryId = parentId
            };

            CategoriesConsumerStub
                .Setup(categories => categories.EditAsync(category))
                .Returns(Task.FromResult(true));

            // Act
            var result = Service.EditCategory(category);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<bool>));
            Assert.AreEqual(expectedResult, result.Result);
        }

        [TestMethod]
        public void NegativeCategoryId_ThrowsValidationException()
        {
            // Arrange
            var id = -1;
            var name = "RenamedTestCategory";
            var parentId = 3;

            var category = new Category
            {
                Id = id,
                Name = name,
                ParentCategoryId = parentId
            };

            // Act
            var result = Service.EditCategory(category);

            // Assert
            Assert.AreEqual(result.Status, TaskStatus.Faulted);
            Assert.IsInstanceOfType(result.Exception.InnerException, typeof(ValidationException));
        }

        [TestMethod]
        public void InvalidCategoryId_ReturnsFalse()
        {
            // Arrange
            var id = 99999;
            var name = "NonExistingCategory";
            var parentId = 3;
            var expectedResult = false;

            var category = new Category
            {
                Id = id,
                Name = name,
                ParentCategoryId = parentId
            };

            CategoriesConsumerStub
                .Setup(categories => categories.EditAsync(category))
                .Returns(Task.FromResult(false));

            // Act
            var result = Service.EditCategory(category);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<bool>));
            Assert.AreEqual(expectedResult, result.Result);
        }
    }
}
