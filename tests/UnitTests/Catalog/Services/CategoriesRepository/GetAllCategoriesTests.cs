using Target = GreenShop.Catalog.Services.Categories.CategoriesRepository;
using Common.Interfaces;
using Common.Models.Categories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UnitTests.Catalog.Services.CategoriesRepository
{
    [TestClass]
    public class GetAllCategoriesTests
    {
        private Mock<ISqlDataAccessor<Category>> CategoriesAccessorStub;
        private Target Service;

        public GetAllCategoriesTests()
        {
            CategoriesAccessorStub = new Mock<ISqlDataAccessor<Category>>();
            Service = new Target(CategoriesAccessorStub.Object);
        }

        [TestMethod]
        public void ReturnsExpectedType()
        {
            // Arrange
            CategoriesAccessorStub
                .Setup(categories => categories.GetAll())
                .Returns(Task.FromResult(ExpectedCategoryList));

            // Act
            var result = Service.GetAllCategories();

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<IEnumerable<Category>>));
        }

        [TestMethod]
        public void ReturnsExpectedCategory()
        {
            // Arrange
            CategoriesAccessorStub
                .Setup(categories => categories.GetAll())
                .Returns(Task.FromResult(ExpectedCategoryList));

            // Act
            var result = Service.GetAllCategories();
            var actualCategory = result.GetAwaiter().GetResult().First();
            var expectedCategory = ExpectedCategoryList.First();

            // Assert
            Assert.AreEqual(result.Result.Count(), ExpectedCategoryList.Count());
            Assert.AreEqual(actualCategory.Id, expectedCategory.Id);
            Assert.AreEqual(actualCategory.Name, expectedCategory.Name);
            Assert.AreEqual(actualCategory.ParentCategoryId, expectedCategory.ParentCategoryId);
        }

        private IEnumerable<Category> ExpectedCategoryList
        {
            get
            {
                var id = 1;
                var name = "TestCategory";
                var parentId = 2;

                var categoriesList = new List<Category>()
                    {
                        new Category
                        {
                            Id = id,
                            Name = name,
                            ParentCategoryId = parentId
                        }
                    };

                return categoriesList;
            }
        }
    }
}
