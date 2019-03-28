using FluentValidation;
using GreenShop.Catalog.DataAccessors.Interfaces;
using GreenShop.Catalog.Models.Categories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using Target = GreenShop.Catalog.Services.Categories.CategoriesRepository;

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
            int id = 1;
            string name = "RenamedTestCategory";
            int parentId = 3;
            bool expectedResult = true;

            Category category = new Category
            {
                Id = id,
                Name = name,
                ParentCategoryId = parentId
            };

            CategoriesAccessorStub
                .Setup(categories => categories.Edit(category))
                .Returns(Task.FromResult(1));

            // Act
            Task<bool> result = Service.EditCategory(category);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<bool>));
            Assert.AreEqual(expectedResult, result.Result);
        }

        [TestMethod]
        public void NegativeCategoryId_ThrowsValidationException()
        {
            // Arrange
            int id = -1;
            string name = "RenamedTestCategory";
            int parentId = 3;

            Category category = new Category
            {
                Id = id,
                Name = name,
                ParentCategoryId = parentId
            };

            // Act
            Task<bool> result = Service.EditCategory(category);

            // Assert
            Assert.AreEqual(result.Status, TaskStatus.Faulted);
            Assert.IsInstanceOfType(result.Exception.InnerException, typeof(ValidationException));
        }

        [TestMethod]
        public void InvalidCategoryId_ReturnsFalse()
        {
            // Arrange
            int id = 99999;
            string name = "NonExistingCategory";
            int parentId = 3;
            bool expectedResult = false;

            Category category = new Category
            {
                Id = id,
                Name = name,
                ParentCategoryId = parentId
            };

            CategoriesAccessorStub
                .Setup(categories => categories.Edit(category))
                .Returns(Task.FromResult(0));

            // Act
            Task<bool> result = Service.EditCategory(category);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<bool>));
            Assert.AreEqual(expectedResult, result.Result);
        }
    }
}
