using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using FluentValidation;
using System.Threading.Tasks;
using GreenShop.Web.Bff.Shopping.Services.Catalog.Interfaces;
using Target = GreenShop.Web.Bff.Shopping.Services.Catalog.CommentService;

namespace UnitTests.WebBffShopping.Services.Catalog.CommentService
{
    [TestClass]
    public class DeleteCommentTests
    {
        private Mock<ICommentConsumer> CommentsAccessorStub;
        private Target CommentService;

        public DeleteCommentTests()
        {
            CommentsAccessorStub = new Mock<ICommentConsumer>();
            CommentService = new Target(CommentsAccessorStub.Object);
        }

        [TestMethod]
        public void ValidComment_ReturnsTrue()
        {
            // Arrange
            var id = 1;
            var expectedResult = true;

            CommentsAccessorStub
                .Setup(Comment => Comment.DeleteAsync(id))
                .Returns(Task.FromResult(true));

            // Act
            var result = CommentService.DeleteComment(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<bool>));
            Assert.AreEqual(expectedResult, result.Result);
        }

        [TestMethod]
        public void NegativeCommentId_ThrowsValidationException()
        {
            // Arrange
            var id = -1;

            // Act
            var result = CommentService.DeleteComment(id);

            // Assert
            Assert.AreEqual(result.Status, TaskStatus.Faulted);
            Assert.IsInstanceOfType(result.Exception.InnerException, typeof(ValidationException));
        }

        [TestMethod]
        public void InvalidCommentId_ReturnsFalse()
        {
            // Arrange 
            var id = 99999;
            var expectedResult = false;

            CommentsAccessorStub
                .Setup(comments => comments.DeleteAsync(id))
                .Returns(Task.FromResult(false));

            // Act
            var result = CommentService.DeleteComment(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<bool>));
            Assert.AreEqual(expectedResult, result.Result);
        }
    }
}
