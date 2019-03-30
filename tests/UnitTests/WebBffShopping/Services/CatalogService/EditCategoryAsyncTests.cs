using GreenShop.Web.Bff.Shopping.Models.Categories;
using GreenShop.Web.Bff.Shopping.Models.Products;
using GreenShop.Web.Bff.Shopping.Services.Catalog.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using Target = GreenShop.Web.Bff.Shopping.Services.CatalogService;

namespace UnitTests.WebBffShopping.Services.CatalogService
{
    [TestClass]
    public class EditCategoryAsyncTests
    {
        private Mock<IConsumer<Category>> CategoriesConsumerStub;
        private Mock<IConsumer<Product>> ProductsConsumerStub;
        private Mock<ICommentsConsumer> CommentsConsumerStub;
        private Target CatalogService;

        public EditCategoryAsyncTests()
        {
            CategoriesConsumerStub = new Mock<IConsumer<Category>>();
            ProductsConsumerStub = new Mock<IConsumer<Product>>();
            CommentsConsumerStub = new Mock<ICommentsConsumer>();
            CatalogService = new Target(CategoriesConsumerStub.Object, ProductsConsumerStub.Object, CommentsConsumerStub.Object);
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

            CategoriesConsumerStub
                .Setup(categories => categories.EditAsync(category))
                .Returns(Task.FromResult(true));

            // Act
            Task<bool> result = CatalogService.EditCategoryAsync(category);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<bool>));
            Assert.AreEqual(expectedResult, result.Result);
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

            CategoriesConsumerStub
                .Setup(categories => categories.EditAsync(category))
                .Returns(Task.FromResult(false));

            // Act
            Task<bool> result = CatalogService.EditCategoryAsync(category);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<bool>));
            Assert.AreEqual(expectedResult, result.Result);
        }
    }
}
