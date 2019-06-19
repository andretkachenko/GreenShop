using GreenShop.Catalog.Api.Domain.Products;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using Target = GreenShop.Catalog.Api.Domain.Products.Product;
using CommentSUT = GreenShop.Catalog.Api.Domain.Products.Comment;

namespace GreenShop.UnitTests.Domain.Products.Product
{
    [TestClass]
    public class AddCommentTests
    {
        [TestMethod]
        public void ValidAuthorIdValidMessage_AddedSuccessfully()
        {
            // Assign
            string initName = "Init Name";
            int initCategoryId = 1;
            int authorId = 1;
            string message = "Test message";
            Target productMock = new Target(initName, initCategoryId);

            // Act
            productMock.AddComment(authorId, message);
            CommentSUT actualComment = productMock.Comments.First();

            // Assert
            Assert.AreEqual(authorId, actualComment.AuthorId);
            Assert.AreEqual(message, actualComment.Message);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ValidAuthorIdNullMessage_ThrowsArgumentException()
        {
            // Assign
            string initName = "Init Name";
            int initCategoryId = 1;
            Target productMock = new Target(initName, initCategoryId);
            int authorId = 1;
            string message = null;

            // Act
            productMock.AddComment(authorId, message);

            // Assert
            Assert.Fail();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ValidAuthorIdEmptyMessage_ThrowsArgumentException()
        {
            // Assign
            string initName = "Init Name";
            int initCategoryId = 1;
            Target productMock = new Target(initName, initCategoryId);
            int authorId = 1;
            string message = "";

            // Act
            productMock.AddComment(authorId, message);

            // Assert
            Assert.Fail();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ValidAuthorIdWhitespaceMessage_ThrowsArgumentException()
        {
            // Assign
            string initName = "Init Name";
            int initCategoryId = 1;
            Target productMock = new Target(initName, initCategoryId);
            int authorId = 1;
            string message = "";

            // Act
            productMock.AddComment(authorId, message);

            // Assert
            Assert.Fail();
        }
    }
}
