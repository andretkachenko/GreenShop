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
    public class GetCategoryTests
    {
        private Mock<ISqlDataAccessor<Category>> CategoriesAccessorStub;
        private Target CategoriesRepository;

        public GetCategoryTests()
        {
            CategoriesAccessorStub = new Mock<ISqlDataAccessor<Category>>();
            CategoriesRepository = new Target(CategoriesAccessorStub.Object);
        }

        [TestMethod]
        public void ValidId_ReturnsExpectedType()
        {
            // Arrange
            int id = 1;

            CategoriesAccessorStub
                .Setup(categories => categories.Get(id))
                .Returns(Task.FromResult(ExpectedValidCategory));

            // Act
            Task<Category> result = CategoriesRepository.GetCategory(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<Category>));
        }

        [TestMethod]
        public void ValidId_ReturnsExpectedCategory()
        {
            // Arrange
            int id = 1;

            CategoriesAccessorStub
                .Setup(categories => categories.Get(id))
                .Returns(Task.FromResult(ExpectedValidCategory));

            // Act
            Task<Category> result = CategoriesRepository.GetCategory(id);
            Category actualCategory = result.GetAwaiter().GetResult();

            // Assert
            Assert.AreEqual(actualCategory.Id, ExpectedValidCategory.Id);
            Assert.AreEqual(actualCategory.Name, ExpectedValidCategory.Name);
            Assert.AreEqual(actualCategory.ParentCategoryId, ExpectedValidCategory.ParentCategoryId);
        }

        [TestMethod]
        public void InvalidId_ReturnsNull()
        {
            // Arrange
            int id = 99999;

            CategoriesAccessorStub
                .Setup(categories => categories.Get(id))
                .Returns(Task.FromResult(ExpectedInvalidCategory));

            // Act
            Task<Category> result = CategoriesRepository.GetCategory(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<Category>));
            Assert.IsNull(result.Result);
        }

        [TestMethod]
        public void NegativeId_ThrowsValidationException()
        {
            // Arrange
            int id = -1;

            // Act
            Task<Category> result = CategoriesRepository.GetCategory(id);

            // Assert
            Assert.AreEqual(result.Status, TaskStatus.Faulted);
            Assert.IsInstanceOfType(result.Exception.InnerException, typeof(ValidationException));
        }

        private Category ExpectedValidCategory
        {
            get
            {
                int id = 1;
                string name = "TestCategory";
                int parentId = 2;

                Category category = new Category
                {
                    Id = id,
                    Name = name,
                    ParentCategoryId = parentId
                };

                return category;
            }
        }

        private Category ExpectedInvalidCategory
        {
            get
            {
                return null;
            }
        }
    }
}
