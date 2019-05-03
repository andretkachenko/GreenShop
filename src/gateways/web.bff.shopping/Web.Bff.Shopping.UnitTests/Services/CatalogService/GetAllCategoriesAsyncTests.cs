using GreenShop.Web.Bff.Shopping.Api.Models.Categories;
using GreenShop.Web.Bff.Shopping.Api.Models.Products;
using GreenShop.Web.Bff.Shopping.Api.Services.Catalog.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Target = GreenShop.Web.Bff.Shopping.Api.Services.CatalogService;

namespace GreenShop.Web.Bff.Shopping.UnitTests.Services.CatalogService
{
    [TestClass]
    public class GetAllCategoriesAsyncTests
    {
        private Mock<IConsumer<Category>> CategoriesConsumerStub;
        private Mock<IConsumer<Product>> ProductsConsumerStub;
        private Mock<ICommentsConsumer> CommentsConsumerStub;
        private Target CatalogService;

        public GetAllCategoriesAsyncTests()
        {
            CategoriesConsumerStub = new Mock<IConsumer<Category>>();
            ProductsConsumerStub = new Mock<IConsumer<Product>>();
            CommentsConsumerStub = new Mock<ICommentsConsumer>();
            CatalogService = new Target(CategoriesConsumerStub.Object, ProductsConsumerStub.Object, CommentsConsumerStub.Object);
        }

        [TestMethod]
        public void ReturnsExpectedType()
        {
            // Arrange
            CategoriesConsumerStub
                .Setup(categories => categories.GetAllAsync())
                .Returns(Task.FromResult(ExpectedCategoryList));

            // Act
            Task<IEnumerable<Category>> result = CatalogService.GetAllCategoriesAsync();

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<IEnumerable<Category>>));
        }

        [TestMethod]
        public void ReturnsExpectedCategory()
        {
            // Arrange
            CategoriesConsumerStub
                .Setup(categories => categories.GetAllAsync())
                .Returns(Task.FromResult(ExpectedCategoryList));

            // Act
            Task<IEnumerable<Category>> result = CatalogService.GetAllCategoriesAsync();
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
