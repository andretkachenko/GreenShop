using FluentValidation;
using GreenShop.MVC.Services.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using Target = GreenShop.MVC.Services.CatalogService;

namespace UnitTests.MVC.Services.CatalogService
{
    public class DeleteCommentAsyncTest
    {
        private Mock<ICatalogConsumer> _catalogConsumer;
        private Target _catalogService;
        public DeleteCommentAsyncTest()
        {
            _catalogConsumer = new Mock<ICatalogConsumer>();
            _catalogService = new Target(_catalogConsumer.Object);
        }

        [TestMethod]
        public void ValidComment_ReturnsTrue()
        {
            // Arrange
            int id = 1;
            bool expectedResult = true;

            _catalogConsumer
                .Setup(Comment => Comment.DeleteCommentAsync(id))
                .Returns(Task.FromResult(true));

            // Act
            Task<bool> result = _catalogService.DeleteCommentAsync(id);

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
            Task<bool> result = _catalogService.DeleteCommentAsync(id);

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

            _catalogConsumer
                .Setup(comments => comments.DeleteCommentAsync(id))
                .Returns(Task.FromResult(false));

            // Act
            Task<bool> result = _catalogService.DeleteCommentAsync(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<bool>));
            Assert.AreEqual(expectedResult, result.Result);
        }
    }
}
