using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using BookWorm.Controllers;
using BookWorm.Models;
using BookWorm.Services.FullTextSearch;
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
            Assert.IsInstanceOfType(model.Model, typeof(Book));
        }

        [TestMethod]
        public void ShouldRedirectToDetailsPageWhenBookIsCreated()
        {
            var book = new Book { Title = "The Book" };
            var bookInformation = new BookInformation(book);

            var mockedRepo = GetMockedRepo();
            mockedRepo.Setup(repo => repo.Create(book)).Returns(new Book { Id = 1, Title = "The Book"});
            var booksController = new BooksController(mockedRepo.Object);

            var viewResult = (RedirectToRouteResult)booksController.Create(bookInformation);

            Assert.IsNotNull(viewResult);
            Assert.AreEqual("Added The Book successfully", booksController.TempData["flashSuccess"]);
            Assert.AreEqual(1, viewResult.RouteValues["id"]);
        }

        private static Mock<Repository> GetMockedRepo()
        {
            var mockedRepo = new Mock<Repository>();
            mockedRepo.Setup(r => r.Search(It.IsAny<Expression<Func<Book, bool>>>())).Returns(new List<Book>());
            return mockedRepo;
        }

        [TestMethod]
        public void ShouldUseRepositoryWhenCreatingABook()
        {
            var book = new Book();
            var bookInformation = new BookInformation(book);

            var mockedRepo = GetMockedRepo();
            mockedRepo.Setup(repo => repo.Create(book)).Returns(new Book());

            var booksController = new BooksController(mockedRepo.Object);

            booksController.Create(bookInformation);
            mockedRepo.Verify(repo => repo.Create(book));
        }

        [TestMethod]
        public void createBookShouldNotSaveWhenIsbnAlreadyExists()
        {
            //Given
            const string alreadyExistingIsbn = "12345";
            var bookAlreadyInDB = new Book() { Id = 1, Isbn = alreadyExistingIsbn };
            var bookToCreate = new Book() { Isbn = alreadyExistingIsbn };
            var mockedRepo = new Mock<Repository>();
            mockedRepo.Setup(repo => repo.Search<Book>(It.IsAny<Expression<Func<Book, bool>>>()))
                .Returns(new List<Book> { bookAlreadyInDB });
            var booksController = new BooksController(mockedRepo.Object);

            //When
            var result = (ViewResult)booksController.Create(new BookInformation(bookToCreate));

            //Then
            mockedRepo.Verify(repo => repo.Create(bookToCreate), Times.Never(), "duplicate ISBN should prevent creating book");
            Assert.AreEqual("The ISBN number already exists", booksController.TempData["flashError"]);
        }



        [TestMethod]
        public void createBookShouldNotSaveWhenIsbnAlreadyExistsWithFakeRepo()
        {
            //Given
            const string alreadyExistingIsbn = "12345";
            var bookAlreadyInDB = new Book() { Id = 1, Isbn = alreadyExistingIsbn };
            var bookToCreate = new Book() { Isbn = alreadyExistingIsbn };
            var fakeRepo = new FakeRepository();
            fakeRepo.Create(bookAlreadyInDB);
            var booksController = new BooksController(fakeRepo);

            //When
            var result = booksController.Create(new BookInformation(bookToCreate));

            //Then
            Assert.AreEqual(fakeRepo.Count<Book>(), 1);
            Assert.AreEqual("The ISBN number already exists", booksController.TempData["flashError"]);
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
            Assert.AreEqual(book, bookInView.Model);
            Assert.AreEqual("A book", booksController.ViewBag.Title);
        }

        [TestMethod]
        public void ShouldListAllBooks()
        {
            var books = new List<Book>();
            Enumerable.Range(1, 10).ToList().ForEach(i=>books.Add(new Book{Id = i}));
            var mockedRepo = new Mock<Repository>();
            mockedRepo.Setup(repo => repo.List<Book>(It.IsAny<int>(), It.IsAny<int>())).Returns(books);
            var booksController = new BooksController(mockedRepo.Object);

            var view = booksController.List();

            var filterInformation = (FilterInformation) view.Model;
            var actualBooks = filterInformation.BookInformations.Select(bookInformation => bookInformation.Model).ToList();
            Assert.IsTrue(books.SequenceEqual(actualBooks));
            Assert.IsFalse(filterInformation.Languages.Any());

            mockedRepo.Verify(repo => repo.List<Book>(It.IsAny<int>(), It.IsAny<int>()), Times.Once());
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
            Assert.AreEqual(book.Title, model.Model.Title);
        }

        [TestMethod]
        public void ShouldUpdateBookOnEditPost()
        {
            var editedBook = new Book { Id = 1, Title = "A book" };
            var editedBookInformation = new BookInformation(editedBook);
            var mockedRepo = GetMockedRepo();
            mockedRepo.Setup(repo => repo.Edit(editedBook));
            var booksController = new BooksController(mockedRepo.Object);

            var viewResult = (RedirectToRouteResult)booksController.Edit(editedBookInformation);

            mockedRepo.Verify(repo => repo.Edit<Book>(editedBook), Times.Once());
            Assert.AreEqual("Updated A book successfully", booksController.TempData["flashSuccess"]);
            Assert.AreEqual(1, viewResult.RouteValues["id"]);
            
        }  
        
        [TestMethod]
        public void ShouldNotUpdateBookOnEditPostIfIsbnIsChangedAndIsDuplicate()
        {
            //Given
            string isbnForAlreadyExistingBook = "12345";
            var bookforEditing = new Book { Id = 1, Title = "A book", Isbn = "11111" };
            var bookAlreadyExistingInDb = new Book { Id = 2, Title = "Another book", Isbn = isbnForAlreadyExistingBook};
            var mockedRepo = new Mock<Repository>();
            mockedRepo.Setup(repo => repo.Search<Book>(It.IsAny<Expression<Func<Book, bool>>>()))
                .Returns(new List<Book> { bookAlreadyExistingInDb, bookforEditing });

            //When   
            bookforEditing.Isbn = isbnForAlreadyExistingBook;
            var booksController = new BooksController(mockedRepo.Object);
            ActionResult viewResult = booksController.Edit(new BookInformation(bookforEditing));

            //Then
            mockedRepo.Verify(repo => repo.Edit<Book>(bookforEditing), Times.Never(), "A book whose ISBN number is edited and duplicates an already existing ISBN number should not be saved to the database");
            Assert.AreEqual("The Book Edit was not saved because the provided ISBN number already exists", booksController.TempData["flashError"], "should return the error message in the model");
         //   Assert.AreEqual("/views/Books/Edit.cshtml", ((ViewResult)viewResult).MasterName, "Should return the book edit view");
            
        }
        
        [TestMethod]
        public void ShouldUpdateBookOnEditPostWithIsbnUnChanged()
        {
            //Given
            var bookforEditing = new Book { Id = 1, Title = "A book", Isbn = "11111" };
            var mockedRepo = new Mock<Repository>();
            var existingBooks = new List<Book> {bookforEditing};
            mockedRepo.Setup(repo => repo.Search<Book>(It.IsAny<Expression<Func<Book, bool>>>()))
                .Returns((Expression<Func<Book, bool>> predicate) => existingBooks.Where(predicate.Compile()).ToList());

            //When   
            bookforEditing.Title = "Changed Book Name";
            var booksController = new BooksController(mockedRepo.Object);
            ActionResult viewResult = booksController.Edit(new BookInformation(bookforEditing));

            //Then
            mockedRepo.Verify(repo => repo.Edit<Book>(bookforEditing), Times.Once(), "Book Should be Edited");
            
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
            var mock = new Mock<IFullTextSearch>();
            var expectedBooks = new List<Book> {books.First(), books.Last()};
            mock.Setup(repo => repo.Search(It.IsAny<string>())).Returns(expectedBooks);
            var booksController = new BooksController(mock.Object);

            var view = (ViewResult)booksController.Search("Book 1");

            var booksInView = ((FilterInformation)view.Model).BookInformations;
            var actualBooks = booksInView.Select(bookInformation => bookInformation.Model).ToList();
            Assert.AreEqual(true, booksController.ViewBag.HideFilter);
            Assert.AreEqual(2, actualBooks.Count());
            Assert.AreEqual("Book 1", actualBooks.First().Title);
            Assert.AreEqual("Book 1", actualBooks.Last().Title);

            mock.Verify(repo => repo.Search(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void BooksControllerListWithSearchQueryShouldPageWithTheQueryText()
        {
            var books = new List<Book>();
            Enumerable.Range(1, 11).ToList().ForEach(i => books.Add(new Book { Title = "Book 1" }));
            var mock = new Mock<IFullTextSearch>();
            var matched = books;
            mock.Setup(repo => repo.Search(It.IsAny<string>())).Returns(matched);
            var booksController = new BooksController(mock.Object);

            var view = (ViewResult)booksController.Search("Book 1");

            var booksInView = ((FilterInformation)view.Model).BookInformations;
            var actualBooks = booksInView.Select(bookInformation => bookInformation.Model).ToList();
            Assert.AreEqual(9, actualBooks.Count());
            Assert.AreEqual(2, booksInView.PageCount);
            Assert.AreEqual("Book 1", booksController.ViewBag.SearchQuery);
        }


        [TestMethod]
        public void BooksControllerListWithSearchQueryShouldAllowAnonymous()
        {
            var booksControllerClass = typeof(BooksController);
            Assert.AreEqual(1, booksControllerClass.GetMethods()
                                                   .First(method => method.Name == "Search")
                                                   .GetCustomAttributes(typeof(AllowAnonymousAttribute), false)
                                                   .Count());
        }

        [TestMethod]
        public void BooksControllerListWithSearchQueryShouldRedirectToDetailsWhenThereIsOnlyOneResult()
        {
            var mock = new Mock<IFullTextSearch>();
            var expectedBooks = new List<Book> { new Book {Id=1, Title = "Book 1" } };
            mock.Setup(repo => repo.Search(It.IsAny<string>())).Returns(expectedBooks);
            var booksController = new BooksController(mock.Object);

            var viewResult = (RedirectToRouteResult)booksController.Search("Book 1");
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
            mockedRepo.Setup(repo => repo.Search<Book>(It.IsAny<Expression<Func<Book, bool>>>(), It.IsAny<int>(), It.IsAny<int>())).Returns(expectedBooks);
            var booksController = new BooksController(mockedRepo.Object);

            var view = (ViewResult)booksController.Language("Zulu");

            var filterInformation = (FilterInformation)view.Model;
            var actualBooks = filterInformation.BookInformations.Select(bookInformation => bookInformation.Model).ToList();
            Assert.AreEqual(2, actualBooks.Count());
            Assert.AreEqual("Zulu", actualBooks.First().Language);
            Assert.AreEqual("Zulu", actualBooks.Last().Language);
            mockedRepo.Verify(repo => repo.Search<Book>(It.IsAny<Expression<Func<Book, bool>>>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once());
            Assert.AreEqual(1, filterInformation.Languages.Count());
            Assert.AreEqual("Zulu", filterInformation.Languages.First());
        }

        [TestMethod]
        public void BooksControllerFilterByAgeRangeShouldReturnBooksInGivenAgeRange()
        {
            var books = new List<Book>();
            Enumerable.Range(1, 8).ToList().ForEach(i => books.Add(new Book { Title = "Book " + i, AgeRange = "3-6" }));
            var book1 = new Book { Title = "Book 9", AgeRange = "0-2" };
            books.Add(book1);
            var book2 = new Book { Title = "Book 10", AgeRange = "0-2" };
            books.Add(book2);
            var mockedRepo = new Mock<Repository>();
            var expectedBooks = new List<Book> { book1, book2 };
            mockedRepo.Setup(repo => repo.Search<Book>(It.IsAny<Expression<Func<Book, bool>>>(),It.IsAny<int>(), It.IsAny<int>())).Returns(expectedBooks);
            var booksController = new BooksController(mockedRepo.Object);

            var view = (ViewResult)booksController.AgeRange("0-2");

            var filterInformation = (FilterInformation)view.Model;
            var actualBooks = filterInformation.BookInformations.Select(bookInformation => bookInformation.Model).ToList();
            Assert.AreEqual(2, actualBooks.Count());
            Assert.AreEqual("0-2", actualBooks.First().AgeRange);
            Assert.AreEqual("0-2", actualBooks.Last().AgeRange);
            mockedRepo.Verify(repo => repo.Search<Book>(It.IsAny<Expression<Func<Book, bool>>>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once());
            Assert.AreEqual(1, filterInformation.AgeRanges.Count());
            Assert.AreEqual("0-2", filterInformation.AgeRanges.First());
        }

        [TestMethod]
        public void BooksControllerFilterByGenreShouldReturnBooksInGivenGenre()
        {
            var books = new List<Book>();
            Enumerable.Range(1, 8).ToList().ForEach(i => books.Add(new Book { Title = "Book " + i, Genre = "Fiction" }));
            var book1 = new Book { Title = "Book 9", Genre = "Poetry" };
            books.Add(book1);
            var book2 = new Book { Title = "Book 10", Genre = "Poetry" };
            books.Add(book2);
            var mockedRepo = new Mock<Repository>();
            var expectedBooks = new List<Book> { book1, book2 };
            mockedRepo.Setup(repo => repo.Search<Book>(It.IsAny<Expression<Func<Book, bool>>>(), It.IsAny<int>(), It.IsAny<int>())).Returns(expectedBooks);
            var booksController = new BooksController(mockedRepo.Object);

            var view = (ViewResult)booksController.Genre("Poetry");

            var filterInformation = (FilterInformation)view.Model;
            var actualBooks = filterInformation.BookInformations.Select(bookInformation => bookInformation.Model).ToList();
            Assert.AreEqual(2, actualBooks.Count());
            Assert.AreEqual("Poetry", actualBooks.First().Genre);
            Assert.AreEqual("Poetry", actualBooks.Last().Genre);
            mockedRepo.Verify(repo => repo.Search<Book>(It.IsAny<Expression<Func<Book, bool>>>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once());
            Assert.AreEqual(1, filterInformation.Genres.Count());
            Assert.AreEqual("Poetry", filterInformation.Genres.First());
        }

        [TestMethod]
        public void BooksControllerFilterByLanguageShouldAllowAnonymous()
        {
            var booksControllerClass = typeof(BooksController);
            Assert.AreEqual(1, booksControllerClass.GetMethods()
                                                   .First(method => method.Name == "Language")
                                                   .GetCustomAttributes(typeof(AllowAnonymousAttribute), false)
                                                   .Count());
        }

        [TestMethod]
        public void BooksControllerFilterByAgeRangeShouldAllowAnonymous()
        {
            var booksControllerClass = typeof(BooksController);
            Assert.AreEqual(1, booksControllerClass.GetMethods()
                                                   .First(method => method.Name == "AgeRange")
                                                   .GetCustomAttributes(typeof(AllowAnonymousAttribute), false)
                                                   .Count());
        }

        [TestMethod]
        public void BooksControllerFilterByGenreShouldAllowAnonymous()
        {
            var booksControllerClass = typeof(BooksController);
            Assert.AreEqual(1, booksControllerClass.GetMethods()
                                                   .First(method => method.Name == "Genre")
                                                   .GetCustomAttributes(typeof(AllowAnonymousAttribute), false)
                                                   .Count());
        }


        class FakeRepository : Repository
        {
            List<Model> objects = new List<Model>();

            public override void Detach<T>(T model) 
            {
                objects.Remove(model);
            }

            public override T Create<T>(T model)
            {
                objects.Add(model);
                return model;
            }

            public override void Delete<T>(int id)
            {
                objects.Remove(Get<T>(id));
            }

            public override T Get<T>(int id)
            {
                return (T)objects.First<Model>(o => o.Id == id);
            }

            public override List<T> List<T>()
            {
                return objects.ConvertAll(a => (T)a);

            }

            public override List<T> List<T>(int perPage)
            {
                return List<T>(0, perPage);
            }

            public override List<T> Search<T>(Expression<Func<T, bool>> predicate)
            {
                return new List<T>(List<T>().Where(predicate.Compile()));
            }

            public override List<T> List<T>(int page, int perPage)
            {
                return objects.GetRange(page * perPage, perPage).ConvertAll(a => (T)a);
            }

            public override List<T> Search<T>(Expression<Func<T, bool>> predicate, int page, int perPage)
            {
                return new List<T>(List<T>(page, perPage).Where(predicate.Compile()));
            }


            public override List<T> Search<T>(Expression<Func<T, bool>> predicate, int perPage)
            {
                return Search(predicate, 0, perPage);
            }

            public override int Count<T>()
            {
                return objects.Count;
            }

            public override int Count<T>(Expression<Func<T, bool>> predicate)
            {
                return Search(predicate).Count();
            }
        }
    }



}
