using Common.Models.Comments;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Web.Bff.Shopping.Services.Catalog.Interfaces;
using Target = Web.Bff.Shopping.Services.Catalog.CommentService;

namespace UnitTests.WebBffShopping.Services.Catalog.CommentService
{
    [TestClass]
    public class GetCommentTests
    {
        private Mock<ICommentConsumer> CommentsAccessorStub;
        private Target CommentService;

        public GetCommentTests()
        {
            CommentsAccessorStub = new Mock<ICommentConsumer>();
            CommentService = new Target(CommentsAccessorStub.Object);
        }

        [TestMethod]
        public void NegativeCommentId_ThrowsValidationException()
        {
            //Arrange
            var id = -1;

            //Act
            var result = CommentService.GetComment(id);

            //Assert
            Assert.AreEqual(result.Status, TaskStatus.Faulted);
            Assert.IsInstanceOfType(result.Exception.InnerException, typeof(ValidationException));
        }

        [TestMethod]
        public void ReturnExpectedComment()
        {
            //Arrange
            var id = 1;
            CommentsAccessorStub
                .Setup(comments => comments.GetAsync(id))
                .Returns(Task.FromResult(ExpectedValidComment));

            //Act 
            var result = CommentService.GetComment(id);
            var comment = result.GetAwaiter().GetResult();

            //Assert
            Assert.AreEqual(comment.Id, ExpectedValidComment.Id);
            Assert.AreEqual(comment.AuthorId, ExpectedValidComment.AuthorId);
            Assert.AreEqual(comment.Message, ExpectedValidComment.Message);
        }

        private Comment ExpectedValidComment
        {
            get
            {
                var id = 1;
                var authorId = 1;
                var parentID = 1;
                var message = "TestMessage";

                var comment = new Comment
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
