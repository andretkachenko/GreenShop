using FluentValidation;
using GreenShop.Web.Mvc.App.Models.Comments;
using GreenShop.Web.Mvc.App.Services.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using Target = GreenShop.Web.Mvc.App.Services.CatalogService;

namespace GreenShop.Web.Mvc.UnitTests.Services.CatalogService
{
    public class AddCommentAsyncTest
    {
        private Mock<ICatalogConsumer> _catalogConsumer;
        private Target _catalogService;
        public AddCommentAsyncTest()
        {
            _catalogConsumer = new Mock<ICatalogConsumer>();
            _catalogService = new Target(_catalogConsumer.Object);
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

            _catalogConsumer
                .Setup(comments => comments.AddCommentAsync(comment))
                .Returns(Task.FromResult(1));

            // Act
            Task<int> result = _catalogService.AddCommentAsync(comment);

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
            Task<int> result = _catalogService.AddCommentAsync(comment);

            //Assert
            Assert.AreEqual(result.Status, TaskStatus.Faulted);
            Assert.IsInstanceOfType(result.Exception.InnerException, typeof(ValidationException));
        }

        [TestMethod]
        public void EmptyMessage_ThrowsValidationException()
        {
            //Arrange
            int authorId = 1;
            string message = "";
            int productId = 1;

            Comment comment = new Comment
            {
                AuthorId = authorId,
                Message = message,
                ProductId = productId
            };

            //Act
            Task<int> result = _catalogService.AddCommentAsync(comment);

            //Assert
            Assert.AreEqual(result.Status, TaskStatus.Faulted);
            Assert.IsInstanceOfType(result.Exception.InnerException, typeof(ValidationException));
        }
    }
}
