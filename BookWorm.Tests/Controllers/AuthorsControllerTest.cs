using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using BookWorm.Controllers;
using BookWorm.Models;
using BookWorm.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BookWorm.Tests.Controllers
{
    [TestClass]
    public class AuthorsControllerTest
    {
        [TestMethod]
        public void DetailsShouldHaveAuthorInfoAndListAuthorBooks()
        {
            var author = new Author()
                {
                    Id = 3,
                    Name = "Author",
                    Biography = "Biography",
                    PictureUrl = "Pic.jpg"
                };
            var books = new List<Book> {new Book(), new Book(), new Book(), new Book()};
            var mockedRepo = new Mock<Repository>();
            mockedRepo.Setup(repo => repo.Get<Author>(3)).Returns(author);
            mockedRepo.Setup(r => r.Search(It.IsAny<Expression<Func<Book, bool>>>())).Returns(books);
            var controller = new AuthorsController(mockedRepo.Object);


            var result = controller.Details(3);
            var model = (AuthorViewModel)result.Model;
            AuthorsContollerTestHelper.AssertEqual(author, model.Author);
            Assert.AreEqual(4, model.Books.Count());
        }

    }
}
