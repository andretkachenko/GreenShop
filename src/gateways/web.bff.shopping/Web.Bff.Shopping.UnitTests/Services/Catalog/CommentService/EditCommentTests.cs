using GreenShop.Web.Bff.Shopping.Api.Models.Comments;
using GreenShop.Web.Bff.Shopping.Api.Services.Catalog.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using Target = GreenShop.Web.Bff.Shopping.Api.Services.Catalog.CommentService;

namespace GreenShop.Web.Bff.Shopping.UnitTests.Services.Catalog.CommentService
{
    [TestClass]
    public class EditCommentTests
    {
        private Mock<ICommentConsumer> CommentsAccessorStub;
        private Target CommentService;

        public EditCommentTests()
        {
            CommentsAccessorStub = new Mock<ICommentConsumer>();
            CommentService = new Target(CommentsAccessorStub.Object);
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

            CommentsAccessorStub
                .Setup(comments => comments.EditAsync(id, message))
                .Returns(Task.FromResult(true));

            // Act
            Task<bool> result = CommentService.EditComment(id, message);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<bool>));
            Assert.AreEqual(expectedResult, result.Result);
        }
    }
}
