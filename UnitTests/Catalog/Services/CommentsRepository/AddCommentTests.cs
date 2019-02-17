using Target = Catalog.Services.Comments.CommentsRepository;
using Common.Interfaces;
using Common.Models.Comments;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using FluentValidation;

namespace UnitTests.Catalog.Services.CommentsRepository
{
    [TestClass]
    public class AddCommentTests
    {
        private Mock<ISqlChildDataAccessor<Comment>> CommentsAccessorStub;
        private Target CommentRepository;

        public AddCommentTests()
        {
            CommentsAccessorStub = new Mock<ISqlChildDataAccessor<Comment>>();
            CommentRepository = new Target(CommentsAccessorStub.Object);
        }

        [TestMethod]
        public void ValidComment_ReturnsTrue()
        {
            //Arrange
            var authorId = 1;
            var message = "TetsCommentMessage";
            var parentId = 1;
            var expectedResult = true;

            var comment = new Comment
            {
                AuthorId = authorId,
                Message = message,
                ProductId = parentId
            };

            CommentsAccessorStub
                .Setup(comments => comments.Add(comment))
                .Returns(Task.FromResult(1));

            // Act
            var result = CommentRepository.AddComment(comment);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<bool>));
            Assert.AreEqual(expectedResult, result.Result);
        }

        [TestMethod]
        public void NegativeCommentsProductId_ThrowsValidationException()
        {
            //Arange
            var authorId = 1;
            var message = "TetsCommentMessage";
            var productId = -1;

            var comment = new Comment
            {
                AuthorId = authorId,
                Message = message,
                ProductId = productId
            };

            //var Act
            var result = CommentRepository.AddComment(comment);

            //Assert
            Assert.AreEqual(result.Status, TaskStatus.Faulted);
            Assert.IsInstanceOfType(result.Exception.InnerException, typeof(ValidationException));
        }
    }
}
