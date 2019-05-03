using GreenShop.Web.Bff.Shopping.Api.Models.Categories;
using GreenShop.Web.Bff.Shopping.Api.Models.Comments;
using GreenShop.Web.Bff.Shopping.Api.Models.Products;
using GreenShop.Web.Bff.Shopping.Api.Services.Catalog.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using Target = GreenShop.Web.Bff.Shopping.Api.Services.CatalogService;

namespace GreenShop.Web.Bff.Shopping.UnitTests.Services.CatalogService
{
    [TestClass]
    public class AddCommentAsyncTests
    {
        private Mock<IConsumer<Category>> CategoriesConsumerStub;
        private Mock<IConsumer<Product>> ProductsConsumerStub;
        private Mock<ICommentsConsumer> CommentsConsumerStub;
        private Target CatalogService;

        public AddCommentAsyncTests()
        {
            CategoriesConsumerStub = new Mock<IConsumer<Category>>();
            ProductsConsumerStub = new Mock<IConsumer<Product>>();
            CommentsConsumerStub = new Mock<ICommentsConsumer>();
            CatalogService = new Target(CategoriesConsumerStub.Object, ProductsConsumerStub.Object, CommentsConsumerStub.Object);
        }

        [TestMethod]
        public void ValidComment_ReturnsId()
        {
            //Arrange
            int authorId = 1;
            string message = "TetsCommentMessage";
            int parentId = 1;
            int expectedResult = 1;

            Comment comment = new Comment
            {
                AuthorId = authorId,
                Message = message,
                ProductId = parentId
            };

            CommentsConsumerStub
                .Setup(comments => comments.AddAsync(comment))
                .Returns(Task.FromResult(1));

            // Act
            Task<int> result = CatalogService.AddCommentAsync(comment);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<int>));
            Assert.AreEqual(expectedResult, result.Result);
        }
    }
}
