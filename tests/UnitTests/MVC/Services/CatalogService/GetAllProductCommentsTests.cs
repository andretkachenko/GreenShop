using Common.Models.Comments;
using FluentValidation;
using GreenShop.MVC.Services.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Target = GreenShop.MVC.Services.CatalogService;

namespace UnitTests.MVC.Services.CatalogService
{
    [TestClass]
    public class GetAllProductCommentsTests
    {
        private Mock<ICatalogConsumer> _catalogConsumer;
        private Target _catalogService;
        public GetAllProductCommentsTests()
        {
            _catalogConsumer = new Mock<ICatalogConsumer>();
            _catalogService = new Target(_catalogConsumer.Object);
        }

        [TestMethod]
        public void NegativeCommentsProductId_ThrowsValidationException()
        {
            //Arrange
            int productId = -1;

            //Act
            Task<IEnumerable<Comment>> result = _catalogService.GetAllProductComments(productId);

            //Assert
            Assert.AreEqual(result.Status, TaskStatus.Faulted);
            Assert.IsInstanceOfType(result.Exception.InnerException, typeof(ValidationException));
        }

        [TestMethod]
        public void ReturnExpectedComment()
        {
            //Arrange
            int id = 1;
            _catalogConsumer
                .Setup(comments => comments.GetallProductCommentsAsync(id))
                .Returns(Task.FromResult(ExpectedCommentsList));

            //Act 
            Task<IEnumerable<Comment>> result = _catalogService.GetAllProductComments(id);
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
