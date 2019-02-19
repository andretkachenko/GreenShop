using Target = Catalog.Services.Comments.CommentsRepository;
using Common.Interfaces;
using Common.Models.Comments;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using FluentValidation;

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
            var id = 1;
            var authorId = 1;
            var message = "EditedCommentMessage";
            var parentId = 1;
            var expectedResult = true;

            var comment = new Comment
            {
                Id = id,
                AuthorId = authorId,
                ProductId = parentId
            };

            CommentsAccessorStub
                .Setup(comments => comments.Edit(id, message))
                .Returns(Task.FromResult(1));

            // Act
            var result = CommentRepository.EditComment(id, message);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<bool>));
            Assert.AreEqual(expectedResult, result.Result);
        }

        [TestMethod]
        public void NegativeCommentId_ThrowsValidationException()
        {
            //Arange
            var id = -1;
            var message = "TetsCommentMessage";

            //Act
            var result = CommentRepository.EditComment(id, message);

            //Assert
            Assert.AreEqual(result.Status, TaskStatus.Faulted);
            Assert.IsInstanceOfType(result.Exception.InnerException, typeof(ValidationException));
        }       

        [TestMethod]
        public void EmptyMessage_ThrowsValidationException()
        {
            //Arange
            var id = 1;
            var message = "";

            //Act
            var result = CommentRepository.EditComment(id, message);

            //Assert
            Assert.AreEqual(result.Status, TaskStatus.Faulted);
            Assert.IsInstanceOfType(result.Exception.InnerException, typeof(ValidationException));
        }
    }
}
