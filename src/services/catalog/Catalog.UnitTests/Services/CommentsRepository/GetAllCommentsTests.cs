using FluentValidation;
using GreenShop.Catalog.DataAccessors.Interfaces;
using GreenShop.Catalog.Models.Comments;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Target = GreenShop.Catalog.Services.Comments.CommentsRepository;

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
        public void NegativeCommentsProductId_ThrowsValidationException()
        {
            //Arrange
            int productId = -1;

            //Act
            Task<IEnumerable<Comment>> result = CommentRepository.GetAllProductComments(productId);

            //Assert
            Assert.AreEqual(result.Status, TaskStatus.Faulted);
            Assert.IsInstanceOfType(result.Exception.InnerException, typeof(ValidationException));
        }

        [TestMethod]
        public void ReturnExpectedComment()
        {
            //Arrange
            int id = 1;
            CommentsAccessorStub
                .Setup(comments => comments.GetAllParentRelated(id))
                .Returns(Task.FromResult(ExpectedCommentsList));

            //Act 
            Task<IEnumerable<Comment>> result = CommentRepository.GetAllProductComments(id);
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

