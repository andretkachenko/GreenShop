using GreenShop.Catalog.DataAccessors.Interfaces;
using GreenShop.Catalog.Models.Categories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Target = GreenShop.Catalog.Services.Categories.CategoriesRepository;

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
            Task<IEnumerable<Category>> result = Service.GetAllCategories();

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
            Task<IEnumerable<Category>> result = Service.GetAllCategories();
            Category actualCategory = result.GetAwaiter().GetResult().First();
            Category expectedCategory = ExpectedCategoryList.First();

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
                int id = 1;
                string name = "TestCategory";
                int parentId = 2;

                List<Category> categoriesList = new List<Category>()
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
