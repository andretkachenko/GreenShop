using FluentValidation;
using GreenShop.Catalog.DataAccessors.Interfaces;
using GreenShop.Catalog.Models.Comments;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using Target = GreenShop.Catalog.Services.Comments.CommentsRepository;

namespace UnitTests.Catalog.Services.CommentsRepository
{
    [TestClass]
    public class DeleteCommentTests
    {
        private Mock<ISqlChildDataAccessor<Comment>> CommentsAccessorStub;
        private Target Service;

        public DeleteCommentTests()
        {
            CommentsAccessorStub = new Mock<ISqlChildDataAccessor<Comment>>();
            Service = new Target(CommentsAccessorStub.Object);
        }

        [TestMethod]
        public void ValidComment_ReturnsTrue()
        {
            // Arrange
            int id = 1;
            bool expectedResult = true;

            CommentsAccessorStub
                .Setup(Comment => Comment.Delete(id))
                .Returns(Task.FromResult(1));

            // Act
            Task<bool> result = Service.DeleteComment(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<bool>));
            Assert.AreEqual(expectedResult, result.Result);
        }

        [TestMethod]
        public void NegativeCommentId_ThrowsValidationException()
        {
            // Arrange
            int id = -1;

            // Act
            Task<bool> result = Service.DeleteComment(id);

            // Assert
            Assert.AreEqual(result.Status, TaskStatus.Faulted);
            Assert.IsInstanceOfType(result.Exception.InnerException, typeof(ValidationException));
        }

        [TestMethod]
        public void InvalidCommentId_ReturnsFalse()
        {
            // Arrange 
            int id = 99999;
            bool expectedResult = false;

            CommentsAccessorStub
                .Setup(comments => comments.Delete(id))
                .Returns(Task.FromResult(0));

            // Act
            Task<bool> result = Service.DeleteComment(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<bool>));
            Assert.AreEqual(expectedResult, result.Result);
        }
    }
}
