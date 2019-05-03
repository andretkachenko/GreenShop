using GreenShop.Web.Bff.Shopping.Api.Models.Categories;
using GreenShop.Web.Bff.Shopping.Api.Models.Products;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using GreenShop.Web.Bff.Shopping.Api.Services.Catalog.Interfaces;
using Target = GreenShop.Web.Bff.Shopping.Api.Services.CatalogService;

namespace GreenShop.Web.Bff.Shopping.UnitTests.Services.CatalogService
{
    [TestClass]
    public class GetCategoryAsyncTests
    {
        private Mock<IConsumer<Category>> CategoriesConsumerStub;
        private Mock<IConsumer<Product>> ProductsConsumerStub;
        private Mock<ICommentsConsumer> CommentsConsumerStub;
        private Target CatalogService;

        public GetCategoryAsyncTests()
        {
            CategoriesConsumerStub = new Mock<IConsumer<Category>>();
            ProductsConsumerStub = new Mock<IConsumer<Product>>();
            CommentsConsumerStub = new Mock<ICommentsConsumer>();
            CatalogService = new Target(CategoriesConsumerStub.Object, ProductsConsumerStub.Object, CommentsConsumerStub.Object);
        }

        [TestMethod]
        public void ValidId_ReturnsExpectedType()
        {
            // Arrange
            int id = 1;

            CategoriesConsumerStub
                .Setup(categories => categories.GetAsync(id))
                .Returns(Task.FromResult(ExpectedValidCategory));

            // Act
            Task<Category> result = CatalogService.GetCategoryAsync(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<Category>));
        }

        [TestMethod]
        public void ValidId_ReturnsExpectedCategory()
        {
            // Arrange
            int id = 1;

            CategoriesConsumerStub
                .Setup(categories => categories.GetAsync(id))
                .Returns(Task.FromResult(ExpectedValidCategory));

            // Act
            Task<Category> result = CatalogService.GetCategoryAsync(id);
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

            CategoriesConsumerStub
                .Setup(categories => categories.GetAsync(id))
                .Returns(Task.FromResult(ExpectedInvalidCategory));

            // Act
            Task<Category> result = CatalogService.GetCategoryAsync(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<Category>));
            Assert.IsNull(result.Result);
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
