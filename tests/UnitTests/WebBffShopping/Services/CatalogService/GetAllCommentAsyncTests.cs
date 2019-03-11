using Common.Models.Categories;
using Common.Models.Comments;
using Common.Models.Products;
using FluentValidation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Bff.Shopping.Services.Catalog.Interfaces;
using Target = Web.Bff.Shopping.Services.CatalogService;

namespace UnitTests.WebBffShopping.Services.CatalogService
{
    [TestClass]
    public class GetAllCommentAsyncTests
    {
        private Mock<IConsumer<Category>> CategoriesConsumerStub;
        private Mock<IConsumer<Product>> ProductsConsumerStub;
        private Mock<ICommentsConsumer> CommentsConsumerStub;
        private Target CatalogService;

        public GetAllCommentAsyncTests()
        {
            CategoriesConsumerStub = new Mock<IConsumer<Category>>();
            ProductsConsumerStub = new Mock<IConsumer<Product>>();
            CommentsConsumerStub = new Mock<ICommentsConsumer>();
            CatalogService = new Target(CategoriesConsumerStub.Object, ProductsConsumerStub.Object, CommentsConsumerStub.Object);
        }

        [TestMethod]
        public void NegativeCommentsProductId_ThrowsValidationException()
        {
            //Arrange
            int productId = -1;

            //Act
            Task<IEnumerable<Comment>> result = CatalogService.GetAllProductCommentsAsync(productId);

            //Assert
            Assert.AreEqual(result.Status, TaskStatus.Faulted);
            Assert.IsInstanceOfType(result.Exception.InnerException, typeof(ValidationException));
        }

        [TestMethod]
        public void ReturnExpectedComment()
        {
            //Arrange
            int id = 1;
            CommentsConsumerStub
                .Setup(comments => comments.GetAllProductRelatedCommentsAsync(id))
                .Returns(Task.FromResult(ExpectedCommentsList));

            //Act 
            Task<IEnumerable<Comment>> result = CatalogService.GetAllProductCommentsAsync(id);
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
