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
        public void ValidComment_ReturnsId()
        {
            //Arrange
            int authorId = 1;
            string message = "TetsCommentMessage";
            int parentId = 1;
            int expectedResult = 1;

            Comment comment = new Comment
            {
                AuthorId = authorId,
                Message = message,
                ProductId = parentId
            };

            CommentsAccessorStub
                .Setup(comments => comments.Add(comment))
                .Returns(Task.FromResult(1));

            // Act
            Task<int> result = CommentRepository.AddComment(comment);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<int>));
            Assert.AreEqual(expectedResult, result.Result);
        }

        [TestMethod]
        public void NegativeCommentsProductId_ThrowsValidationException()
        {
            //Arrange
            int authorId = 1;
            string message = "TetsCommentMessage";
            int productId = -1;

            Comment comment = new Comment
            {
                AuthorId = authorId,
                Message = message,
                ProductId = productId
            };

            //Act
            Task<int> result = CommentRepository.AddComment(comment);

            //Assert
            Assert.AreEqual(result.Status, TaskStatus.Faulted);
            Assert.IsInstanceOfType(result.Exception.InnerException, typeof(ValidationException));
        }

        [TestMethod]
        public void EmptyMessage_ThrowsValidationException()
        {
            //Arrange
            int authorId = 1;
            string message = string.Empty;
            int productId = 1;

            Comment comment = new Comment
            {
                AuthorId = authorId,
                Message = message,
                ProductId = productId
            };

            //Act
            Task<int> result = CommentRepository.AddComment(comment);

            //Assert
            Assert.AreEqual(result.Status, TaskStatus.Faulted);
            Assert.IsInstanceOfType(result.Exception.InnerException, typeof(ValidationException));
        }
    }
}
