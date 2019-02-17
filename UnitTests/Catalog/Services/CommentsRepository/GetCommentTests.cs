using Target = Catalog.Services.Comments.CommentsRepository;
using Common.Interfaces;
using Common.Models.Comments;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace UnitTests.Catalog.Services.CommentsRepository
{
    [TestClass]
    public class GetCommentTests
    {
        private Mock<ISqlChildDataAccessor<Comment>> CommentsAccessorStub;
        private Target CommentRepository;

        public GetCommentTests()
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
                .Setup(comments => comments.Get(id))
                .Returns(Task.FromResult(ExpectedValidComment));

            //Act 
            var result =  CommentRepository.GetComment(id);
            var comment = result.GetAwaiter().GetResult();

            //Assert
            Assert.AreEqual(comment.Id, ExpectedValidComment.Id);
            Assert.AreEqual(comment.AuthorId, ExpectedValidComment.AuthorId);
            Assert.AreEqual(comment.Message, ExpectedValidComment.Message);
        }

        private Comment ExpectedValidComment
        {
            get
            {
                var id = 1;
                var authorId = 1;
                var parentID = 1;
                var message = "TestMessage";

                var comment = new Comment
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
