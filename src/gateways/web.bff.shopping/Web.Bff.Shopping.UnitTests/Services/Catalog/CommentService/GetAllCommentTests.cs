using GreenShop.Web.Bff.Shopping.Api.Models.Comments;
using GreenShop.Web.Bff.Shopping.Api.Services.Catalog.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Target = GreenShop.Web.Bff.Shopping.Api.Services.Catalog.CommentService;

namespace GreenShop.Web.Bff.Shopping.UnitTests.Services.Catalog.CommentService
{
    [TestClass]
    public class GetAllCommentTests
    {
        private Mock<ICommentConsumer> CommentsAccessorStub;
        private Target CommentService;

        public GetAllCommentTests()
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
                .Setup(comments => comments.GetAllProductRelatedCommentsAsync(id))
                .Returns(Task.FromResult(ExpectedCommentsList));

            //Act 
            Task<IEnumerable<Comment>> result = CommentService.GetAllProductComments(id);
            Comment comment = result.GetAwaiter().GetResult().First();
            Comment expectedComment = ExpectedCommentsList.First();

            //Assert
            Assert.AreEqual(comment.Id, expectedComment.Id);
            Assert.AreEqual(comment.AuthorId, expectedComment.AuthorId);
            Assert.AreEqual(comment.Message, expectedComment.Message);
        }

        private IEnumerable<Comment> ExpectedCommentsList
        {
            get
            {
                int id = 1;
                int parentId = 1;
                int authorId = 1;
                string message = "TestMessage";

                List<Comment> commentsList = new List<Comment>
                {
                    new Comment
                    {
                        Id=id,
                        ProductId=parentId,
                        AuthorId=authorId,
                        Message=message
                    }
                };

                return commentsList;
            }
        }
    }
}
