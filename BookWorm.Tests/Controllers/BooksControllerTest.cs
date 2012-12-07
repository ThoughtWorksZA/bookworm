using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BookWorm.Controllers;
using BookWorm.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BookWorm.Tests.Controllers
{
    [TestClass]
    public class BooksControllerTest
    {
        [TestMethod]
        public void ShouldReturnCreatePageOnGetCreate()
        {
            var booksController = new BooksController();

            var createABookView = booksController.New();
            var model = createABookView.Model;

            Assert.AreEqual("Add a Book", booksController.ViewBag.Title);
            Assert.IsInstanceOfType(model, typeof(Book));
        }

        [TestMethod]
        public void ShouldReturnPageWhenBookIsCreated()
        {
            var book = new Book { Title = "The Book" };

            var mockedRepo = new Mock<Repository>();
            mockedRepo.Setup(repo => repo.Create(book)).Returns(new Book { Id = 1, Title = "The Book" });
            var booksController = new BooksController(mockedRepo.Object);

            var viewResult = booksController.New(book);

            Assert.IsNotNull(viewResult);
            Assert.AreEqual("Added The Book successfully", booksController.TempData["flash"]);
            Assert.AreEqual(1, viewResult.RouteValues["id"]);
        }

        [TestMethod]
        public void ShouldUseRepositoryWhenCreatingABook()
        {
            var book = new Book();
            var mockedRepo = new Mock<Repository>();
            mockedRepo.Setup(repo => repo.Create(book)).Returns(new Book());

            var booksController = new BooksController(mockedRepo.Object);

            booksController.New(book);
            mockedRepo.Verify(repo => repo.Create(book));
        }

        [TestMethod]
        public void ShouldDisplayBookOnIndex()
        {
            var book = new Book {Id = 1, Title = "A book"};
            var mockedRepo = new Mock<Repository>();
            mockedRepo.Setup(repo => repo.Get<Book>(book.Id)).Returns(book);
            var booksController = new BooksController(mockedRepo.Object);

            var view = booksController.Index(1);

            var bookInView = (Book) view.Model;
            Assert.IsInstanceOfType(view, typeof(ViewResult));
            mockedRepo.Verify(repo => repo.Get<Book>(book.Id), Times.Once());
            Assert.AreEqual(book, bookInView);
        }

        [TestMethod]
        public void ShouldListAllBooks()
        {
            var books = new List<Book>();
            Enumerable.Range(1, 10).ToList().ForEach(i=>books.Add(new Book{Id = i}));
            var mockedRepo = new Mock<Repository>();
            mockedRepo.Setup(repo => repo.List<Book>()).Returns(books);
            var booksController = new BooksController(mockedRepo.Object);

            var view = booksController.List();

            var booksInView = (List<Book>) view.Model;
            mockedRepo.Verify(repo => repo.List<Book>(), Times.Once());
            Assert.AreEqual(books, booksInView);
        }
    }
}
