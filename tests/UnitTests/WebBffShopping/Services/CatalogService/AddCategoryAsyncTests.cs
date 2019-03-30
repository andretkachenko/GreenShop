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
    public class AddCategoryAsyncTests
    {
        private Mock<IConsumer<Category>> CategoriesConsumerStub;
        private Mock<IConsumer<Product>> ProductsConsumerStub;
        private Mock<ICommentsConsumer> CommentsConsumerStub;
        private Target CatalogService;

        public AddCategoryAsyncTests()
        {
            CategoriesConsumerStub = new Mock<IConsumer<Category>>();
            ProductsConsumerStub = new Mock<IConsumer<Product>>();
            CommentsConsumerStub = new Mock<ICommentsConsumer>();
            CatalogService = new Target(CategoriesConsumerStub.Object, ProductsConsumerStub.Object, CommentsConsumerStub.Object);
        }

        [TestMethod]
        public void ValidCategory_ReturnsExpectedId()
        {
            // Arrange
            int id = 1;
            string name = "TestName";
            int parentCategoryId = 2;

            Category expectedCategory = new Category
            {
                Id = id,
                Name = name,
                ParentCategoryId = parentCategoryId
            };

            CategoriesConsumerStub
                .Setup(categories => categories.AddAsync(expectedCategory))
                .Returns(Task.FromResult(id));

            // Act
            Task<int> result = CatalogService.AddCategoryAsync(expectedCategory);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<int>));
            Assert.AreEqual(id, result.Result);
        }
    }
}
