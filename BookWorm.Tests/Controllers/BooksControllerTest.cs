using System.Web.Mvc;
using BookWorm.Controllers;
using BookWorm.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BookWorm.Tests.Controllers
{
    [TestClass]
    public class BooksControllerTest
    {
        [TestMethod]
        public void CreatingABookReturnsAPage()
        {
            var book = new Book { Title = "The Book" };

            var booksController = new BooksController();

            ViewResult viewResult = booksController.New(book);

            Assert.IsNotNull(viewResult);
            Assert.AreEqual("Created book 'The Book'", booksController.ViewBag.SuccessMessage);
            Assert.AreEqual("Success", booksController.ViewBag.Title);
        }

        [TestMethod]
        public void CanReturnAPageToCreateABook()
        {
            var booksController = new BooksController();

            var createABookPage = booksController.New();
            var model = createABookPage.Model;

            Assert.AreEqual("Add a Book", booksController.ViewBag.Title);
            Assert.IsInstanceOfType(model, typeof(Book));
        }
    }
}
