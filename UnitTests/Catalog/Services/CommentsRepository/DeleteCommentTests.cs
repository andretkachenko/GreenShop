using Target = Catalog.Services.Comments.CommentsRepository;
using Common.Interfaces;
using Common.Models.Comments;
using FluentValidation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

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
            var id = 1;
            var expectedResult = true;

            CommentsAccessorStub
                .Setup(Comment => Comment.Delete(id))
                .Returns(Task.FromResult(1));

            // Act
            var result = Service.DeleteComment(id);

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
            var result = Service.DeleteComment(id);

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
                .Setup(comments => comments.Delete(id))
                .Returns(Task.FromResult(0));

            // Act
            var result = Service.DeleteComment(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<bool>));
            Assert.AreEqual(expectedResult, result.Result);
        }
    }

    [TestClass]
    public class AddCommentTests
    {
        private Mock<ISqlChildDataAccessor<Comment>> CommentsAccessorStub;
        private Target Service;

        public AddCommentTests()
        {
            CommentsAccessorStub = new Mock<ISqlChildDataAccessor<Comment>>();
            Service = new Target(CommentsAccessorStub.Object);
        }
    }

    [TestClass]
    public class EditCommentTests
    {
        private Mock<ISqlChildDataAccessor<Comment>> CommentsAccessorStub;
        private Target Service;

        public EditCommentTests()
        {
            CommentsAccessorStub = new Mock<ISqlChildDataAccessor<Comment>>();
            Service = new Target(CommentsAccessorStub.Object);
        }
    }

    [TestClass]
    public class GetCommentTests
    {
        private Mock<ISqlChildDataAccessor<Comment>> CommentsAccessorStub;
        private Target Service;

        public GetCommentTests()
        {
            CommentsAccessorStub = new Mock<ISqlChildDataAccessor<Comment>>();
            Service = new Target(CommentsAccessorStub.Object);
        }
    }

    [TestClass]
    public class GetAllTests
    {
        private Mock<ISqlChildDataAccessor<Comment>> CommentsAccessorStub;
        private Target Service;

        public GetAllTests()
        {
            CommentsAccessorStub = new Mock<ISqlChildDataAccessor<Comment>>();
            Service = new Target(CommentsAccessorStub.Object);
        }
    }
}
