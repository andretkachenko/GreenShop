using Catalog.Services.Comments;
using Common.Interfaces;
using Common.Models.Comments;
using FluentValidation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace UnitTests.Catalog.Services.Comments
{

    [TestClass]
    public class AddCommentTests
    {
        private Mock<IChildtDataAccessor<Comment>> CommentsAccessorMock;
        private CommentsRepository Service;

        public AddCommentTests()
        {
            CommentsAccessorMock = new Mock<IChildtDataAccessor<Comment>>();
            Service = new CommentsRepository(CommentsAccessorMock.Object);
        }

        [TestMethod]
        public void ValidComment_ReturnsTrue()
        {
            // Arrange
            var productId = 3;
            var id = 1;
            var expectedResult = true;

            CommentsAccessorMock
                .Setup(Comment => Comment.Delete(productId, id))
                .Returns(Task.FromResult(1));

            // Act
            var result = Service.DeleteComment(productId, id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<bool>));
            Assert.AreEqual(expectedResult, result.Result);
        }

        [TestMethod]
        public void NegativeCommentId_ThrowsValidationException()
        {
            // Arrange
            var productId = 3;
            var id = -1;

            // Act
            var result = Service.DeleteComment(productId, id);

            // Assert
            Assert.AreEqual(result.Status, TaskStatus.Faulted);
            Assert.IsInstanceOfType(result.Exception.InnerException, typeof(ValidationException));
        }

        [TestMethod]
        public void InvalidCommentId_ReturnsFalse()
        {
            // Arrange 
            var productId = 3;
            var id = 99999;
            var expectedResult = false;

            CommentsAccessorMock
                .Setup(comments => comments.Delete(productId, id))
                .Returns(Task.FromResult(0));

            // Act
            var result = Service.DeleteComment(productId, id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<bool>));
            Assert.AreEqual(expectedResult, result.Result);
        }
    }
}
