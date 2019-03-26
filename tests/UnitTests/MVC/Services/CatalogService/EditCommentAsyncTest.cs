using Common.Models.Comments;
using FluentValidation;
using GreenShop.MVC.Services.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using Target = GreenShop.MVC.Services.CatalogService;

namespace UnitTests.MVC.Services.CatalogService
{
    public class EditCommentAsyncTest
    {
        private Mock<ICatalogConsumer> _catalogConsumer;
        private Target _catalogService;
        public EditCommentAsyncTest()
        {
            _catalogConsumer = new Mock<ICatalogConsumer>();
            _catalogService = new Target(_catalogConsumer.Object);
        }

        [TestMethod]
        public void ValidComment_ReturnsTrue()
        {
            //Arrange
            int id = 1;
            int authorId = 1;
            string message = "EditedCommentMessage";
            int parentId = 1;
            bool expectedResult = true;

            Comment comment = new Comment
            {
                Id = id,
                AuthorId = authorId,
                ProductId = parentId
            };

            _catalogConsumer
                .Setup(comments => comments.EditCommentAsync(id, message))
                .Returns(Task.FromResult(true));

            // Act
            Task<bool> result = _catalogService.EditCommentAsync(comment);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<bool>));
            Assert.AreEqual(expectedResult, result.Result);
        }

        [TestMethod]
        public void NegativeCommentId_ThrowsValidationException()
        {
            //Arrange
            int id = -1;
            string message = "TetsCommentMessage";

            Comment comment = new Comment
            {
                Id = id,
                Message = message
            };

            //Act
            Task<bool> result = _catalogService.EditCommentAsync(comment);

            //Assert
            Assert.AreEqual(result.Status, TaskStatus.Faulted);
            Assert.IsInstanceOfType(result.Exception.InnerException, typeof(ValidationException));
        }

        [TestMethod]
        public void EmptyMessage_ThrowsValidationException()
        {
            //Arrange
            int id = 1;
            string message = "";

            Comment comment = new Comment
            {
                Id = id,
                Message = message
            };

            //Act
            var result = _catalogService.EditCommentAsync(comment);

            //Assert
            Assert.AreEqual(result.Status, TaskStatus.Faulted);
            Assert.IsInstanceOfType(result.Exception.InnerException, typeof(ValidationException));
        }
    }
}
