using Common.Models.Comments;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using FluentValidation;
using System.Threading.Tasks;
using Web.Bff.Shopping.Services.Catalog.Interfaces;
using Target = Web.Bff.Shopping.Services.Catalog.CommentService;

namespace UnitTests.WebBffShopping.Services.Catalog.CommentService
{
    [TestClass]
    public class AddCommentTests
    {
        private Mock<ICommentConsumer> CommentsAccessorStub;
        private Target CommentService;

        public AddCommentTests()
        {
            CommentsAccessorStub = new Mock<ICommentConsumer>();
            CommentService = new Target(CommentsAccessorStub.Object);
        }

        [TestMethod]
        public void ValidComment_ReturnsId()
        {
            //Arrange
            var authorId = 1;
            var message = "TetsCommentMessage";
            var parentId = 1;
            var expectedResult = 1;

            var comment = new Comment
            {
                AuthorId = authorId,
                Message = message,
                ProductId = parentId
            };

            CommentsAccessorStub
                .Setup(comments => comments.AddAsync(comment))
                .Returns(Task.FromResult(1));

            // Act
            var result = CommentService.AddComment(comment);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<int>));
            Assert.AreEqual(expectedResult, result.Result);
        }

        [TestMethod]
        public void NegativeCommentsProductId_ThrowsValidationException()
        {
            //Arrange
            var authorId = 1;
            var message = "TetsCommentMessage";
            var productId = -1;

            var comment = new Comment
            {
                AuthorId = authorId,
                Message = message,
                ProductId = productId
            };

            //Act
            var result = CommentService.AddComment(comment);

            //Assert
            Assert.AreEqual(result.Status, TaskStatus.Faulted);
            Assert.IsInstanceOfType(result.Exception.InnerException, typeof(ValidationException));
        }

        [TestMethod]
        public void EmptyMessage_ThrowsValidationException()
        {
            //Arrange
            var authorId = 1;
            var message = "";
            var productId = 1;

            var comment = new Comment
            {
                AuthorId = authorId,
                Message = message,
                ProductId = productId
            };

            //Act
            var result = CommentService.AddComment(comment);

            //Assert
            Assert.AreEqual(result.Status, TaskStatus.Faulted);
            Assert.IsInstanceOfType(result.Exception.InnerException, typeof(ValidationException));
        }
    }
}
