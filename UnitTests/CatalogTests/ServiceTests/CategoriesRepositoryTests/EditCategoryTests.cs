using Catalog.Services.Categories;
using Common.Interfaces;
using Common.Models.Categories;
using FluentValidation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace UnitTests.CatalogTests.ServiceTests.CategoriesRepositoryTests
{
    [TestClass]
    public class EditCategoryTests
    {
        private Mock<ISqlDataAccessor<Category>> CategoriesAccessorMock;
        private CategoriesRepository Service;

        public EditCategoryTests()
        {
            CategoriesAccessorMock = new Mock<ISqlDataAccessor<Category>>();
            Service = new CategoriesRepository(CategoriesAccessorMock.Object);
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

            CategoriesAccessorMock
                .Setup(categories => categories.Edit(category))
                .Returns(Task.FromResult(1));

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

            CategoriesAccessorMock
                .Setup(categories => categories.Edit(category))
                .Returns(Task.FromResult(0));

            // Act
            var result = Service.EditCategory(category);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<bool>));
            Assert.AreEqual(expectedResult, result.Result);
        }
    }
}
