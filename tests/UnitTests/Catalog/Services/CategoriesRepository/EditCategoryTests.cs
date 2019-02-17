using Target = Catalog.Services.Categories.CategoriesRepository;
using Common.Interfaces;
using Common.Models.Categories;
using FluentValidation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace UnitTests.Catalog.Services.CategoriesRepository
{
    [TestClass]
    public class EditCategoryTests
    {
        private Mock<ISqlDataAccessor<Category>> CategoriesAccessorStub;
        private Target Service;

        public EditCategoryTests()
        {
            CategoriesAccessorStub = new Mock<ISqlDataAccessor<Category>>();
            Service = new Target(CategoriesAccessorStub.Object);
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

            CategoriesAccessorStub
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

            CategoriesAccessorStub
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
