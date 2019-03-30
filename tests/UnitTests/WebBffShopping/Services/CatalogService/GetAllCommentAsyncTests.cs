using GreenShop.Web.Bff.Shopping.Models.Categories;
using GreenShop.Web.Bff.Shopping.Models.Comments;
using GreenShop.Web.Bff.Shopping.Models.Products;
using GreenShop.Web.Bff.Shopping.Services.Catalog.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Target = GreenShop.Web.Bff.Shopping.Services.CatalogService;

namespace UnitTests.WebBffShopping.Services.CatalogService
{
    [TestClass]
    public class GetAllCommentAsyncTests
    {
        private Mock<IConsumer<Category>> CategoriesConsumerStub;
        private Mock<IConsumer<Product>> ProductsConsumerStub;
        private Mock<ICommentsConsumer> CommentsConsumerStub;
        private Target CatalogService;

        public GetAllCommentAsyncTests()
        {
            CategoriesConsumerStub = new Mock<IConsumer<Category>>();
            ProductsConsumerStub = new Mock<IConsumer<Product>>();
            CommentsConsumerStub = new Mock<ICommentsConsumer>();
            CatalogService = new Target(CategoriesConsumerStub.Object, ProductsConsumerStub.Object, CommentsConsumerStub.Object);
        }

        [TestMethod]
        public void ReturnExpectedComment()
        {
            //Arrange
            int id = 1;
            CommentsConsumerStub
                .Setup(comments => comments.GetAllProductRelatedCommentsAsync(id))
                .Returns(Task.FromResult(ExpectedCommentsList));

            //Act 
            Task<IEnumerable<Comment>> result = CatalogService.GetAllProductCommentsAsync(id);
            Comment comment = result.GetAwaiter().GetResult().First();
            Comment expectedComment = ExpectedCommentsList.First();

            //Assert
            Assert.AreEqual(comment.Id, expectedComment.Id);
            Assert.AreEqual(comment.AuthorId, expectedComment.AuthorId);
            Assert.AreEqual(comment.Message, expectedComment.Message);
        }

        private IEnumerable<Comment> ExpectedCommentsList
        {
            get
            {
                int id = 1;
                int parentId = 1;
                int authorId = 1;
                string message = "TestMessage";

                List<Comment> commentsList = new List<Comment>
                {
                    new Comment
                    {
                        Id=id,
                        ProductId=parentId,
                        AuthorId=authorId,
                        Message=message
                    }
                };

                return commentsList;
            }
        }
    }
}
