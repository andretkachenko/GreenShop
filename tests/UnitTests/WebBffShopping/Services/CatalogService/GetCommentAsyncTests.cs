using Common.Models.Categories;
using Common.Models.Comments;
using Common.Models.Products;
using FluentValidation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using Web.Bff.Shopping.Services.Catalog.Interfaces;
using Target = Web.Bff.Shopping.Services.CatalogService;

namespace UnitTests.WebBffShopping.Services.CatalogService
{
    [TestClass]
    public class GetCommentAsyncTests
    {
        private Mock<IConsumer<Category>> CategoriesConsumerStub;
        private Mock<IConsumer<Product>> ProductsConsumerStub;
        private Mock<ICommentsConsumer> CommentsConsumerStub;
        private Target CatalogService;

        public GetCommentAsyncTests()
        {
            CategoriesConsumerStub = new Mock<IConsumer<Category>>();
            ProductsConsumerStub = new Mock<IConsumer<Product>>();
            CommentsConsumerStub = new Mock<ICommentsConsumer>();
            CatalogService = new Target(CategoriesConsumerStub.Object, ProductsConsumerStub.Object, CommentsConsumerStub.Object);
        }

        [TestMethod]
        public void NegativeCommentId_ThrowsValidationException()
        {
            //Arrange
            int id = -1;

            //Act
            Task<Comment> result = CatalogService.GetCommentAsync(id);

            //Assert
            Assert.AreEqual(result.Status, TaskStatus.Faulted);
            Assert.IsInstanceOfType(result.Exception.InnerException, typeof(ValidationException));
        }

        [TestMethod]
        public void ReturnExpectedComment()
        {
            //Arrange
            int id = 1;
            CommentsConsumerStub
                .Setup(comments => comments.GetAsync(id))
                .Returns(Task.FromResult(ExpectedValidComment));

            //Act 
            Task<Comment> result = CatalogService.GetCommentAsync(id);
            Comment comment = result.GetAwaiter().GetResult();

            //Assert
            Assert.AreEqual(comment.Id, ExpectedValidComment.Id);
            Assert.AreEqual(comment.AuthorId, ExpectedValidComment.AuthorId);
            Assert.AreEqual(comment.Message, ExpectedValidComment.Message);
        }

        private Comment ExpectedValidComment
        {
            get
            {
                int id = 1;
                int authorId = 1;
                int parentID = 1;
                string message = "TestMessage";

                Comment comment = new Comment
                {
                    Id = id,
                    AuthorId = authorId,
                    ProductId = parentID,
                    Message = message
                };
                return comment;
            }
        }
    }
}
