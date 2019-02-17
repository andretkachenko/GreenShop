using Target = Catalog.Services.Comments.CommentsRepository;
using Common.Interfaces;
using Common.Models.Comments;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace UnitTests.Catalog.Services.CommentsRepository
{
    [TestClass]
    public class GetAllCommentsTests
    {
        private Mock<ISqlChildDataAccessor<Comment>> CommentsAccessorStub;
        private Target CommentRepository;

        public GetAllCommentsTests()
        {
            CommentsAccessorStub = new Mock<ISqlChildDataAccessor<Comment>>();
            CommentRepository = new Target(CommentsAccessorStub.Object);
        }

        [TestMethod]
        public void ReturnExpectedComment()
        {
            //Arrange
            var id = 1;
            CommentsAccessorStub
                .Setup(comments => comments.GetAllParentRelated(id))
                .Returns(Task.FromResult(ExpectedCommentsList));

            //Act 
            var result = CommentRepository.GetAllProductComments(id);
            var comment = result.GetAwaiter().GetResult().First();
            var expectedComment = ExpectedCommentsList.First();

            //Assert
            Assert.AreEqual(comment.Id, expectedComment.Id);
            Assert.AreEqual(comment.AuthorId, expectedComment.AuthorId);
            Assert.AreEqual(comment.Message, expectedComment.Message);
        }

        private IEnumerable<Comment> ExpectedCommentsList
        {
            get
            {
                var id = 1;
                var parentId = 1;
                var authorId = 1;
                var message = "TestMessage";

                var commentsList = new List<Comment>
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

