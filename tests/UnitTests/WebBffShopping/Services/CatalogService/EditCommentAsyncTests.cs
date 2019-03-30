using GreenShop.Web.Bff.Shopping.Models.Categories;
using GreenShop.Web.Bff.Shopping.Models.Comments;
using GreenShop.Web.Bff.Shopping.Models.Products;
using GreenShop.Web.Bff.Shopping.Services.Catalog.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using Target = GreenShop.Web.Bff.Shopping.Services.CatalogService;

namespace UnitTests.WebBffShopping.Services.CatalogService
{
    [TestClass]
    public class EditCommentAsyncTests
    {
        private Mock<IConsumer<Category>> CategoriesConsumerStub;
        private Mock<IConsumer<Product>> ProductsConsumerStub;
        private Mock<ICommentsConsumer> CommentsConsumerStub;
        private Target CatalogService;

        public EditCommentAsyncTests()
        {
            CategoriesConsumerStub = new Mock<IConsumer<Category>>();
            ProductsConsumerStub = new Mock<IConsumer<Product>>();
            CommentsConsumerStub = new Mock<ICommentsConsumer>();
            CatalogService = new Target(CategoriesConsumerStub.Object, ProductsConsumerStub.Object, CommentsConsumerStub.Object);
        }

        [TestMethod]
        public void ValidComment_ReturnsTrue()
        {
            //Arrange
            int id = 1;
            int authorId = 1;
            string message = "EditedCommentMessage";
            int parentId = 1;
            bool expectedResult = true;

            Comment comment = new Comment
            {
                Id = id,
                AuthorId = authorId,
                ProductId = parentId
            };

            CommentsConsumerStub
                .Setup(comments => comments.EditAsync(id, message))
                .Returns(Task.FromResult(true));

            // Act
            Task<bool> result = CatalogService.EditCommentAsync(id, message);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<bool>));
            Assert.AreEqual(expectedResult, result.Result);
        }
    }
}
