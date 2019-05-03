using GreenShop.Web.Bff.Shopping.Api.Models.Comments;
using GreenShop.Web.Bff.Shopping.Api.Services.Catalog.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using Target = GreenShop.Web.Bff.Shopping.Api.Services.Catalog.CommentService;

namespace GreenShop.Web.Bff.Shopping.UnitTests.Services.Catalog.CommentService
{
    [TestClass]
    public class GetCommentTests
    {
        private Mock<ICommentConsumer> CommentsAccessorStub;
        private Target CommentService;

        public GetCommentTests()
        {
            CommentsAccessorStub = new Mock<ICommentConsumer>();
            CommentService = new Target(CommentsAccessorStub.Object);
        }

        [TestMethod]
        public void ReturnExpectedComment()
        {
            //Arrange
            int id = 1;
            CommentsAccessorStub
                .Setup(comments => comments.GetAsync(id))
                .Returns(Task.FromResult(ExpectedValidComment));

            //Act 
            Task<Comment> result = CommentService.GetComment(id);
            Comment comment = result.GetAwaiter().GetResult();

            //Assert
            Assert.AreEqual(comment.Id, ExpectedValidComment.Id);
            Assert.AreEqual(comment.AuthorId, ExpectedValidComment.AuthorId);
            Assert.AreEqual(comment.Message, ExpectedValidComment.Message);
        }

        private Comment ExpectedValidComment
        {
            get
            {
                int id = 1;
                int authorId = 1;
                int parentID = 1;
                string message = "TestMessage";

                Comment comment = new Comment
                {
                    Id = id,
                    AuthorId = authorId,
                    ProductId = parentID,
                    Message = message
                };
                return comment;
            }
        }
    }
}
