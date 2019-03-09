using Common.Models.Comments;
using FluentValidation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using Web.Bff.Shopping.Services.Catalog.Interfaces;
using Target = Web.Bff.Shopping.Services.Catalog.CommentService;

namespace UnitTests.WebBffShopping.Services.Catalog.CommentService
{
    [TestClass]
    public class EditCommentTests
    {
        private Mock<ICommentConsumer> CommentsAccessorStub;
        private Target CommentService;

        public EditCommentTests()
        {
            CommentsAccessorStub = new Mock<ICommentConsumer>();
            CommentService = new Target(CommentsAccessorStub.Object);
        }

        [TestMethod]
        public void ValidComment_ReturnsTrue()
        {
            //Arrange
            var id = 1;
            var authorId = 1;
            var message = "EditedCommentMessage";
            var parentId = 1;
            var expectedResult = true;

            var comment = new Comment
            {
                Id = id,
                AuthorId = authorId,
                ProductId = parentId
            };

            CommentsAccessorStub
                .Setup(comments => comments.EditAsync(id, message))
                .Returns(Task.FromResult(true));

            // Act
            var result = CommentService.EditComment(id, message);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<bool>));
            Assert.AreEqual(expectedResult, result.Result);
        }

        [TestMethod]
        public void NegativeCommentId_ThrowsValidationException()
        {
            //Arrange
            var id = -1;
            var message = "TetsCommentMessage";

            //Act
            var result = CommentService.EditComment(id, message);

            //Assert
            Assert.AreEqual(result.Status, TaskStatus.Faulted);
            Assert.IsInstanceOfType(result.Exception.InnerException, typeof(ValidationException));
        }

        [TestMethod]
        public void EmptyMessage_ThrowsValidationException()
        {
            //Arrange
            var id = 1;
            var message = "";

            //Act
            var result = CommentService.EditComment(id, message);

            //Assert
            Assert.AreEqual(result.Status, TaskStatus.Faulted);
            Assert.IsInstanceOfType(result.Exception.InnerException, typeof(ValidationException));
        }
    }
}
