using Catalog.Services.Categories;
using Common.Interfaces;
using Common.Models.Categories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UnitTests.CatalogTests.ServiceTests.CategoriesServiceTests
{
    [TestClass]
    public class GetAllCategoriesTests
    {
        private Mock<IParentDataAccessor<Category>> CategoriesAccessorMock;
        private CategoriesService Service;

        public GetAllCategoriesTests()
        {
            CategoriesAccessorMock = new Mock<IParentDataAccessor<Category>>();
            Service = new CategoriesService(CategoriesAccessorMock.Object);
        }

        [TestMethod]
        public void ReturnsExpectedType()
        {
            // Arrange
            CategoriesAccessorMock
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
            CategoriesAccessorMock
                .Setup(categories => categories.GetAll())
                .Returns(Task.FromResult(ExpectedCategoryList));

            // Act
            var result = Service.GetAllCategories();

            // Assert
            Assert.AreEqual(result.Result.First(), ExpectedCategoryList.First());
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
