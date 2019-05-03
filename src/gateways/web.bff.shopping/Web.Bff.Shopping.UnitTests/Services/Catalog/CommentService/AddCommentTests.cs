using GreenShop.Web.Bff.Shopping.Api.Models.Comments;
using GreenShop.Web.Bff.Shopping.Api.Services.Catalog.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using Target = GreenShop.Web.Bff.Shopping.Api.Services.Catalog.CommentService;

namespace GreenShop.Web.Bff.Shopping.UnitTests.Services.Catalog.CommentService
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
                .Setup(comments => comments.AddAsync(comment))
                .Returns(Task.FromResult(1));

            // Act
            Task<int> result = CommentService.AddComment(comment);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<int>));
            Assert.AreEqual(expectedResult, result.Result);
        }
    }
}
