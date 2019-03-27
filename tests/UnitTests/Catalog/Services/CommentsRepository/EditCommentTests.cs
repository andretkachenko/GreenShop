using Common.Interfaces;
using Common.Models.Comments;
using FluentValidation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using Target = GreenShop.Catalog.Services.Comments.CommentsRepository;

namespace UnitTests.Catalog.Services.CommentsRepository
{
    [TestClass]
    public class EditCommentTests
    {
        private Mock<ISqlChildDataAccessor<Comment>> CommentsAccessorStub;
        private Target CommentRepository;

        public EditCommentTests()
        {
            CommentsAccessorStub = new Mock<ISqlChildDataAccessor<Comment>>();
            CommentRepository = new Target(CommentsAccessorStub.Object);
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
                .Setup(comments => comments.Edit(id, message))
                .Returns(Task.FromResult(1));

            // Act
            Task<bool> result = CommentRepository.EditComment(id, message);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<bool>));
            Assert.AreEqual(expectedResult, result.Result);
        }

        [TestMethod]
        public void NegativeCommentId_ThrowsValidationException()
        {
            //Arrange
            int id = -1;
            string message = "TetsCommentMessage";

            //Act
            Task<bool> result = CommentRepository.EditComment(id, message);

            //Assert
            Assert.AreEqual(result.Status, TaskStatus.Faulted);
            Assert.IsInstanceOfType(result.Exception.InnerException, typeof(ValidationException));
        }

        [TestMethod]
        public void EmptyMessage_ThrowsValidationException()
        {
            //Arrange
            int id = 1;
            string message = "";

            //Act
            Task<bool> result = CommentRepository.EditComment(id, message);

            //Assert
            Assert.AreEqual(result.Status, TaskStatus.Faulted);
            Assert.IsInstanceOfType(result.Exception.InnerException, typeof(ValidationException));
        }
    }
}
