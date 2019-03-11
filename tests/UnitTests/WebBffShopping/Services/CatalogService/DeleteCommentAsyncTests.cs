using Common.Models.Categories;
using Common.Models.Products;
using FluentValidation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using Web.Bff.Shopping.Services.Catalog.Interfaces;
using Target = Web.Bff.Shopping.Services.CatalogService;

namespace UnitTests.WebBffShopping.Services.CatalogService
{
    [TestClass]
    public class DeleteCommentAsyncTests
    {
        private Mock<IConsumer<Category>> CategoriesConsumerStub;
        private Mock<IConsumer<Product>> ProductsConsumerStub;
        private Mock<ICommentsConsumer> CommentsConsumerStub;
        private Target CatalogService;

        public DeleteCommentAsyncTests()
        {
            CategoriesConsumerStub = new Mock<IConsumer<Category>>();
            ProductsConsumerStub = new Mock<IConsumer<Product>>();
            CommentsConsumerStub = new Mock<ICommentsConsumer>();
            CatalogService = new Target(CategoriesConsumerStub.Object, ProductsConsumerStub.Object, CommentsConsumerStub.Object);
        }

        [TestMethod]
        public void ValidComment_ReturnsTrue()
        {
            // Arrange
            int id = 1;
            bool expectedResult = true;

            CommentsConsumerStub
                .Setup(Comment => Comment.DeleteAsync(id))
                .Returns(Task.FromResult(true));

            // Act
            Task<bool> result = CatalogService.DeleteCommentAsync(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<bool>));
            Assert.AreEqual(expectedResult, result.Result);
        }

        [TestMethod]
        public void NegativeCommentId_ThrowsValidationException()
        {
            // Arrange
            int id = -1;

            // Act
            Task<bool> result = CatalogService.DeleteCommentAsync(id);

            // Assert
            Assert.AreEqual(result.Status, TaskStatus.Faulted);
            Assert.IsInstanceOfType(result.Exception.InnerException, typeof(ValidationException));
        }

        [TestMethod]
        public void InvalidCommentId_ReturnsFalse()
        {
            // Arrange 
            int id = 99999;
            bool expectedResult = false;

            CommentsConsumerStub
                .Setup(comments => comments.DeleteAsync(id))
                .Returns(Task.FromResult(false));

            // Act
            Task<bool> result = CatalogService.DeleteCommentAsync(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<bool>));
            Assert.AreEqual(expectedResult, result.Result);
        }
    }
}
