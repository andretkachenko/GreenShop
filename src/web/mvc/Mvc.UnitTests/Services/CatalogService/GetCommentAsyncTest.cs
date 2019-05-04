using FluentValidation;
using GreenShop.Web.Mvc.App.Models.Comments;
using GreenShop.Web.Mvc.App.Services.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using Target = GreenShop.Web.Mvc.App.Services.CatalogService;

namespace GreenShop.Web.Mvc.UnitTests.Services.CatalogService
{
    public class GetCommentAsyncTest
    {
        private Mock<ICatalogConsumer> _catalogConsumer;
        private Target _catalogService;
        public GetCommentAsyncTest()
        {
            _catalogConsumer = new Mock<ICatalogConsumer>();
            _catalogService = new Target(_catalogConsumer.Object);
        }

        [TestMethod]
        public void NegativeCommentId_ThrowsValidationException()
        {
            //Arrange
            int id = -1;

            //Act
            Task<Comment> result = _catalogService.GetCommentAsync(id);

            //Assert
            Assert.AreEqual(result.Status, TaskStatus.Faulted);
            Assert.IsInstanceOfType(result.Exception.InnerException, typeof(ValidationException));
        }

        [TestMethod]
        public void ReturnExpectedComment()
        {
            //Arrange
            int id = 1;
            _catalogConsumer
                .Setup(comments => comments.GetCommentAsync(id))
                .Returns(Task.FromResult(ExpectedValidComment));

            //Act 
            Task<Comment> result = _catalogService.GetCommentAsync(id);
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
