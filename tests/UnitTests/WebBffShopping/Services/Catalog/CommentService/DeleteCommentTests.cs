using GreenShop.Web.Bff.Shopping.Services.Catalog.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
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
            int id = 1;
            bool expectedResult = true;

            CommentsAccessorStub
                .Setup(Comment => Comment.DeleteAsync(id))
                .Returns(Task.FromResult(true));

            // Act
            Task<bool> result = CommentService.DeleteComment(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<bool>));
            Assert.AreEqual(expectedResult, result.Result);
        }

        [TestMethod]
        public void InvalidCommentId_ReturnsFalse()
        {
            // Arrange 
            int id = 99999;
            bool expectedResult = false;

            CommentsAccessorStub
                .Setup(comments => comments.DeleteAsync(id))
                .Returns(Task.FromResult(false));

            // Act
            Task<bool> result = CommentService.DeleteComment(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<bool>));
            Assert.AreEqual(expectedResult, result.Result);
        }
    }
}
