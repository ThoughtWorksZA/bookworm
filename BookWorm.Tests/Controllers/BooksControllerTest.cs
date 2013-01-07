using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BookWorm.Controllers;
using BookWorm.Models;
using BookWorm.ViewModels;
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

            var createABookView = booksController.Create();
            var model = (BookInformation)createABookView.Model;

            Assert.AreEqual("Add a Book", booksController.ViewBag.Title);
            Assert.IsInstanceOfType(model, typeof(BookInformation));
            Assert.IsInstanceOfType(model.Book, typeof(Book));
        }

        [TestMethod]
        public void ShouldRedirectToDetailsPageWhenBookIsCreated()
        {
            var book = new Book { Title = "The Book" };
            var bookInformation = new BookInformation(book);

            var mockedRepo = new Mock<Repository>();
            mockedRepo.Setup(repo => repo.Create(book)).Returns(new Book { Id = 1, Title = "The Book"});
            var booksController = new BooksController(mockedRepo.Object);

            var viewResult = (RedirectToRouteResult)booksController.Create(bookInformation);

            Assert.IsNotNull(viewResult);
            Assert.AreEqual("Added The Book successfully", booksController.TempData["flashSuccess"]);
            Assert.AreEqual(1, viewResult.RouteValues["id"]);
        }

        [TestMethod]
        public void ShouldUseRepositoryWhenCreatingABook()
        {
            var book = new Book();
            var bookInformation = new BookInformation(book);

            var mockedRepo = new Mock<Repository>();
            mockedRepo.Setup(repo => repo.Create(book)).Returns(new Book());

            var booksController = new BooksController(mockedRepo.Object);

            booksController.Create(bookInformation);
            mockedRepo.Verify(repo => repo.Create(book));
        }

        [TestMethod]
        public void CreateBookShouldNotSaveWhenBookIsInvalid()
        {
            var book = new Book();
            var bookInformation = new BookInformation(book);
            var mockedRepo = new Mock<Repository>();
            var booksController = new BooksController(mockedRepo.Object);
            mockedRepo.Setup(repo => repo.Create(book)).Returns(book);
            booksController.ModelState.AddModelError("test error","test exception");

            var result = (ViewResult)booksController.Create(bookInformation);

            mockedRepo.Verify(repo => repo.Create(book), Times.Never(), "failing model validation should prevent creating book");
            Assert.AreEqual("There were problems saving this book", booksController.TempData["flashError"]);
        }

        [TestMethod]
        public void ShouldDisplayBookDetails()
        {
            var book = new Book {Id = 1, Title = "A book"};
            var mockedRepo = new Mock<Repository>();
            mockedRepo.Setup(repo => repo.Get<Book>(book.Id)).Returns(book);
            var booksController = new BooksController(mockedRepo.Object);

            var view = booksController.Details(1);

            var bookInView = (BookInformation) view.Model;
            Assert.IsInstanceOfType(view, typeof(ViewResult));
            mockedRepo.Verify(repo => repo.Get<Book>(book.Id), Times.Once());
            Assert.AreEqual(book, bookInView.Book);
            Assert.AreEqual("A book", booksController.ViewBag.Title);
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

            var booksInView = (List<BookInformation>) view.Model;
            var actualBooks = booksInView.Select(bookInformation => bookInformation.Book).ToList();
            Assert.IsTrue(books.SequenceEqual(actualBooks));

            mockedRepo.Verify(repo => repo.List<Book>(), Times.Once());
        }

        [TestMethod]
        public void ShouldReturnEditPageOnGetEdit()
        {
            var book = new Book { Id = 1, Title = "A book" };
            var mockedRepo = new Mock<Repository>();
            mockedRepo.Setup(repo => repo.Get<Book>(book.Id)).Returns(book);
            var booksController = new BooksController(mockedRepo.Object);

            var editABookView = booksController.Edit(1);
            var model = (BookInformation)editABookView.Model;
           
            Assert.AreEqual("PUT", booksController.ViewBag.Method);
            Assert.AreEqual("Edit a Book", booksController.ViewBag.Title);
            Assert.AreEqual(book.Title, model.Book.Title);
        }

        [TestMethod]
        public void ShouldUpdateBookOnEditPost()
        {
            var editedBook = new Book { Id = 1, Title = "A book" };
            var editedBookInformation = new BookInformation(editedBook);
            var mockedRepo = new Mock<Repository>();
            mockedRepo.Setup(repo => repo.Edit(editedBook));
            var booksController = new BooksController(mockedRepo.Object);

            var viewResult = (RedirectToRouteResult)booksController.Edit(editedBookInformation);

            mockedRepo.Verify(repo => repo.Edit<Book>(editedBook), Times.Once());
            Assert.AreEqual("Updated A book successfully", booksController.TempData["flashSuccess"]);
            Assert.AreEqual(1, viewResult.RouteValues["id"]);
            
        }

        [TestMethod]
        public void EditBookShouldNotSaveWhenBookIsInvalid()
        {
            var book = new Book();
            var bookInformation = new BookInformation(book);
            var mockedRepo = new Mock<Repository>();
            var booksController = new BooksController(mockedRepo.Object);
            mockedRepo.Setup(repo => repo.Edit(book));
            booksController.ModelState.AddModelError("test error", "test exception");

            var result = (ViewResult)booksController.Edit(bookInformation);

            mockedRepo.Verify(repo => repo.Edit(book), Times.Never(), "failing model validation should prevent updating book");
            Assert.AreEqual("There were problems saving this book", booksController.TempData["flashError"]);
        }

        [TestMethod]
        public void ShouldDeleteBookAndShowListOfBooks()
        {
            var mockedRepo = new Mock<Repository>();
            mockedRepo.Setup(repo => repo.Delete<Book>(1));
            var booksController = new BooksController(mockedRepo.Object);

            var viewResult = booksController.Delete(1);
            mockedRepo.Verify(repo => repo.Delete<Book>(1));
            Assert.AreEqual("Book successfully deleted", booksController.TempData["flashSuccess"]);
            Assert.AreEqual("List", viewResult.RouteValues["action"]);
        }

        [TestMethod]
        public void BooksControllerShouldRequireAuthorization()
        {
            var booksControllerClass = typeof(BooksController);
            Assert.AreEqual(1, booksControllerClass.GetCustomAttributes(typeof(AuthorizeAttribute), false).Count());
        }
        [TestMethod]
        public void BooksControllerListShouldAllowAnonymous()
        {
            var booksControllerClass = typeof(BooksController);
            Assert.AreEqual(1, booksControllerClass.GetMethods()
                                                   .First(method => method.Name == "List")
                                                   .GetCustomAttributes(typeof(AllowAnonymousAttribute), false)
                                                   .Count());
        }

        [TestMethod]
        public void BooksControllerDetailsShouldAllowAnonymous()
        {
            var booksControllerClass = typeof(BooksController);
            Assert.AreEqual(1, booksControllerClass.GetMethods()
                                                   .First(method => method.Name == "Details")
                                                   .GetCustomAttributes(typeof(AllowAnonymousAttribute), false)
                                                   .Count());
        }

        [TestMethod]
        public void BooksControllerListWithSearchQueryShouldSearchByGivenCondition()
        {
            var books = new List<Book>();
            Enumerable.Range(1, 9).ToList().ForEach(i => books.Add(new Book { Title = "Book " + i}));
            books.Add(new Book { Title = "Book 1" });
            var mockedRepo = new Mock<Repository>();
            var expectedBooks = new List<Book> {books.First(), books.Last()};
            mockedRepo.Setup(repo => repo.Search<Book>(It.IsAny<Func<Book, bool>>())).Returns(expectedBooks);
            var booksController = new BooksController(mockedRepo.Object);

            var view = (ViewResult)booksController.List("Book 1");

            var booksInView = (List<BookInformation>)view.Model;
            var actualBooks = booksInView.Select(bookInformation => bookInformation.Book).ToList();
            Assert.AreEqual(2, actualBooks.Count());
            Assert.AreEqual("Book 1", actualBooks.First().Title);
            Assert.AreEqual("Book 1", actualBooks.Last().Title);

            mockedRepo.Verify(repo => repo.Search<Book>(It.IsAny<Func<Book, bool>>()), Times.Once());
        }


        [TestMethod]
        public void BooksControllerListWithSearchQueryShouldAllowAnonymous()
        {
            var booksControllerClass = typeof(BooksController);
            Assert.AreEqual(1, booksControllerClass.GetMethods()
                                                   .First(method => method.Name == "List" && method.GetParameters().Count() == 1)
                                                   .GetCustomAttributes(typeof(AllowAnonymousAttribute), false)
                                                   .Count());
        }

        [TestMethod]
        public void BooksControllerListWithSearchQueryShouldRedirectToDetailsWhenThereIsOnlyOneResult()
        {
            var mockedRepo = new Mock<Repository>();
            var expectedBooks = new List<Book> { new Book {Id=1, Title = "Book 1" } };
            mockedRepo.Setup(repo => repo.Search<Book>(It.IsAny<Func<Book, bool>>())).Returns(expectedBooks);
            var booksController = new BooksController(mockedRepo.Object);

            var viewResult = (RedirectToRouteResult)booksController.List("Book 1");
            Assert.AreEqual(1, viewResult.RouteValues["id"]);


        }

        [TestMethod]
        public void BooksControllerFilterByLanguageShouldReturnBooksInGivenLanguage()
        {
            var books = new List<Book>();
            Enumerable.Range(1, 8).ToList().ForEach(i => books.Add(new Book { Title = "Book " + i, Language = "Venda"}));
            var book1 = new Book {Title = "Book 9", Language = "Zulu"};
            books.Add(book1);
            var book2 = new Book {Title = "Book 10", Language = "Zulu"};
            books.Add(book2);
            var mockedRepo = new Mock<Repository>();
            var expectedBooks = new List<Book> { book1, book2 };
            mockedRepo.Setup(repo => repo.Search<Book>(It.IsAny<Func<Book, bool>>())).Returns(expectedBooks);
            var booksController = new BooksController(mockedRepo.Object);

            var view = (ViewResult)booksController.FilterByLanguage("Zulu");

            var booksInView = (List<BookInformation>)view.Model;
            var actualBooks = booksInView.Select(bookInformation => bookInformation.Book).ToList();
            Assert.AreEqual(2, actualBooks.Count());
            Assert.AreEqual("Zulu", actualBooks.First().Language);
            Assert.AreEqual("Zulu", actualBooks.Last().Language);

            mockedRepo.Verify(repo => repo.Search<Book>(It.IsAny<Func<Book, bool>>()), Times.Once());
        }

        [TestMethod]
        public void BooksControllerFilterByLanguageShouldRedirectToDetailsWhenThereIsOnlyOneResult()
        {
            var mockedRepo = new Mock<Repository>();
            var expectedBooks = new List<Book> { new Book { Id = 1, Title = "Book 1", Language = "Zulu"} };
            mockedRepo.Setup(repo => repo.Search<Book>(It.IsAny<Func<Book, bool>>())).Returns(expectedBooks);
            var booksController = new BooksController(mockedRepo.Object);

            var viewResult = (RedirectToRouteResult)booksController.FilterByLanguage("Zulu");
            Assert.AreEqual(1, viewResult.RouteValues["id"]);


        }

        [TestMethod]
        public void BooksControllerFilterByLanguageShouldAllowAnonymous()
        {
            var booksControllerClass = typeof(BooksController);
            Assert.AreEqual(1, booksControllerClass.GetMethods()
                                                   .First(method => method.Name == "FilterByLanguage" && method.GetParameters().Count() == 1)
                                                   .GetCustomAttributes(typeof(AllowAnonymousAttribute), false)
                                                   .Count());
        }

    }

}
