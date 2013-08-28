using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BookWorm.Controllers;
using BookWorm.Models;
using BookWorm.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BookWorm.Tests.Controllers.Integration
{
    [TestClass]
    public class BooksControllerTest:ControllerInMemoryRavenTestBase
    {
        [TestMethod]
        public void ShouldFilterByLanguage()
        {
            using (var session = _documentStore.OpenSession())
            {
                var repository = new Repository(session);
                Enumerable.Range(1, 8).ToList().ForEach(i => repository.Create(new Book { Title = "Book " + i, Language = "Venda" }));
                var book1 = new Book { Title = "Book 9", Language = "Zulu" };
                repository.Create(book1);
                var book2 = new Book { Title = "Book 10", Language = "Xhosa" };
                repository.Create(book2);
                session.SaveChanges();
            
                Assert.AreEqual(10, repository.Count<Book>());

                var expectedBooks = new List<Book> { book1, book2 };
                var booksController = new BooksController(repository);

                var view = (ViewResult)booksController.Filter(new List<string>() { "Zulu", "Xhosa" }, new List<string>(), new List<string>(), new List<string>());

                var filterInformation = (FilterInformation)view.Model;
                var actualBooks = filterInformation.BookInformations.Select(bookInformation => bookInformation.Model).ToList();
                Assert.AreEqual(2, actualBooks.Count());
                Assert.AreEqual(expectedBooks.First().Language, actualBooks.First().Language);
                Assert.AreEqual(expectedBooks.Last().Language, actualBooks.Last().Language);
                Assert.AreEqual(2, filterInformation.Languages.Count());
                Assert.AreEqual("Zulu", filterInformation.Languages.First());
                Assert.AreEqual("Xhosa", filterInformation.Languages.Last());
            }
        }

        [TestMethod]
        public void ShouldFilterByLanguageAndAge()
        {
            using (var session = _documentStore.OpenSession())
            {
                var repository = new Repository(session);
                Enumerable.Range(1, 8).ToList().ForEach(i => repository.Create(new Book { Title = "Book " + i, Language = "Venda" }));
                var book1 = new Book { Title = "Book 9", Language = "Zulu", AgeRange = "0-2", Genre = "Fiction"};
                repository.Create(book1);
                var book2 = new Book { Title = "Book 10", Language = "Zulu", AgeRange = "3-5" };
                repository.Create(book2);
                session.SaveChanges();

                Assert.AreEqual(10, repository.Count<Book>());

                var expectedBooks = new List<Book> { book1 };
                var booksController = new BooksController(repository);

                var view = (ViewResult)booksController.Filter(new List<string>() { "Zulu" }, new List<string>() { "0-2" }, new List<string>() { "Fiction" }, new List<string>());

                var filterInformation = (FilterInformation)view.Model;
                var actualBooks = filterInformation.BookInformations.Select(bookInformation => bookInformation.Model).ToList();
                Assert.AreEqual(1, actualBooks.Count());
                Assert.AreEqual(expectedBooks.First().Language, actualBooks.First().Language);
                Assert.AreEqual(expectedBooks.First().AgeRange, actualBooks.First().AgeRange);
                Assert.AreEqual("Zulu", filterInformation.Languages.First());
                Assert.AreEqual("0-2", filterInformation.AgeRanges.First());
            }
        }

        [TestMethod]
        public void ShouldFilterByGenre()
        {
            using (var session = _documentStore.OpenSession())
            {
                var repository = new Repository(session);
                Enumerable.Range(1, 8).ToList().ForEach(i => repository.Create(new Book { Title = "Book " + i, Language = "Venda", Genre = "Poetry"}));
                var book1 = new Book { Title = "Book 9", Language = "Zulu", AgeRange = "0-2", Genre = "Picture Books"};
                repository.Create(book1);
                var book2 = new Book { Title = "Book 10", Language = "Zulu", AgeRange = "3-5", Genre = "Fiction" };
                repository.Create(book2);
                session.SaveChanges();

                Assert.AreEqual(10, repository.Count<Book>());

                var expectedBooks = new List<Book> { book1 };
                var booksController = new BooksController(repository);

                var view = (ViewResult)booksController.Filter(new List<string>(), new List<string>(), new List<string> { "Picture Books" }, new List<string>());

                var filterInformation = (FilterInformation)view.Model;
                var actualBooks = filterInformation.BookInformations.Select(bookInformation => bookInformation.Model).ToList();
                Assert.AreEqual(1, actualBooks.Count());
                Assert.AreEqual(expectedBooks.First().Language, actualBooks.First().Language);
                Assert.AreEqual(expectedBooks.First().AgeRange, actualBooks.First().AgeRange);
                Assert.AreEqual(expectedBooks.First().Genre, actualBooks.First().Genre);
                Assert.AreEqual("Picture Books", filterInformation.Genres.First());
            }
        }

        [TestMethod]
        public void BooksControllerFilterShouldReturnNothingWhenNoFiltersAreGiven()
        {
            using (var session = _documentStore.OpenSession())
            {
                var repository = new Repository(session);
                var books = new List<Book>();
                Enumerable.Range(1, 8)
                          .ToList()
                          .ForEach(
                              i => books.Add(new Book {Title = "Book " + i, Language = "Venda", Genre = "Non-Fiction"}));
                var book1 = new Book {Title = "Book 9", Language = "Zulu"};
                books.Add(book1);
                var book2 = new Book {Title = "Book 10", Language = "Zulu"};
                books.Add(book2);
                var booksController = new BooksController(repository);

                var view = (ViewResult) booksController.Filter(null, null, null, null);

                var filterInformation = (FilterInformation) view.Model;
                Assert.IsFalse(filterInformation.BookInformations.Any());
                Assert.IsFalse(filterInformation.Languages.Any());
            }
        }

        [TestMethod]
        public void ShouldFilterByLanguageWithMoreThan1024Books()
        {
            using (var session = _documentStore.OpenSession())
            {
                var repository = new Repository(session);
                Enumerable.Range(1, 2000).ToList().ForEach(
                    i => repository.Create(new Book
                        {
                            Title = "Book " + i, 
                            Language = "Venda", 
                            Genre = "Fiction",
                            AgeRange = "0-2",
                            CreatedAt = new DateTime(2000,1,1).AddDays(i)
                        }));
                session.SaveChanges();

                var booksController = new BooksController(repository);

                var view = (ViewResult)booksController.Filter(new List<string>() { "Venda" }, new List<string> { "0-2" }, new List<string> { "Fiction" }, new List<string>(), 1, 9);

                var filterInformation = (FilterInformation)view.Model;
                var actualBooks = filterInformation.BookInformations.Select(bookInformation => bookInformation.Model).ToList();
                Assert.AreEqual(9, actualBooks.Count());
                Assert.AreEqual("Book 2000", actualBooks.First().Title);
                Assert.AreEqual("Book 1992", actualBooks.Last().Title);

                view = (ViewResult)booksController.Filter(new List<string>() { "Venda" }, new List<string> { "0-2" }, new List<string> { "Fiction" }, new List<string>(), 223, 9);

                filterInformation = (FilterInformation)view.Model;
                actualBooks = filterInformation.BookInformations.Select(bookInformation => bookInformation.Model).ToList();
                Assert.AreEqual(2, actualBooks.Count());
                Assert.AreEqual("Book 2", actualBooks.First().Title);
                Assert.AreEqual("Book 1", actualBooks.Last().Title);
            }
        }

        [TestMethod]
        public void ShouldFilterByPostType()
        {
            using (var session = _documentStore.OpenSession())
            {
                var repository = new Repository(session);
                var titleBookWithReview = "Book with Review";
                repository.Create(new Book
                {
                    Title = titleBookWithReview,
                    Language = "Venda",
                    Genre = "Fiction",
                    AgeRange = "0-2",
                    Posts = {new BookPost()
                        {
                            Title = "a Review",
                            Content = "A review",
                            Type = BookPost.BookPostType.Reviews
                        }
                    }
                });

                repository.Create(new Book
                {
                    Title = "Book without post",
                    Language = "Venda",
                    Genre = "Fiction",
                    AgeRange = "0-2"
                });

                repository.Create(new Book
                {
                    Title = "Book with Events",
                    Language = "Venda",
                    Genre = "Fiction",
                    AgeRange = "0-2",
                    Posts = {new BookPost()
                        {
                            Title = "an Event",
                            Content = "An Events",
                            Type = BookPost.BookPostType.Events
                        }
                    }
                });

                session.SaveChanges();

                var bookController = new BooksController(repository);
                var result = bookController.PostType(postTypes: "Reviews");
                var viewResult = (ViewResult)result;
                var filterInformation = (FilterInformation)viewResult.Model;
                var books = filterInformation.BookInformations.Select(b => b.Model).ToList();
                Assert.AreEqual(1, books.Count);
                Assert.AreEqual(titleBookWithReview, books[0].Title);
            }
        }

        [TestMethod]
        public void ShouldFilterByPostTypeUsingFilterAction()
        {
            using (var session = _documentStore.OpenSession())
            {
                var repository = new Repository(session);
                repository.Create(new Book
                {
                    Title = "Book with Review",
                    Language = "Venda",
                    Genre = "Fiction",
                    AgeRange = "0-2",
                    Posts = {new BookPost()
                        {
                            Title = "a Review",
                            Content = "A review",
                            Type = BookPost.BookPostType.Reviews
                        }
                    }
                });

                repository.Create(new Book
                {
                    Title = "Book without post",
                    Language = "Venda",
                    Genre = "Fiction",
                    AgeRange = "0-2"
                });

                repository.Create(new Book
                {
                    Title = "Book with Events",
                    Language = "Venda",
                    Genre = "Fiction",
                    AgeRange = "0-2",
                    Posts = {new BookPost()
                        {
                            Title = "an Event",
                            Content = "An Event",
                            Type = BookPost.BookPostType.Events
                        }
                    }
                });
                session.SaveChanges();

                var booksController = new BooksController(repository);

                var view = (ViewResult)booksController.Filter(new List<string>(), new List<string>(), new List<string>(), new List<string>() { "Reviews" });

                var filterInformation = (FilterInformation)view.Model;
                var actualBooks = filterInformation.BookInformations.Select(bookInformation => bookInformation.Model).ToList();
                Assert.AreEqual(1, actualBooks.Count());
                Assert.AreEqual("Book with Review", actualBooks.First().Title);
            }
        }

        [TestMethod]
        public void ShouldFilterByLanguageAndPostTypeUsingFilterAction()
        {
            using (var session = _documentStore.OpenSession())
            {
                var repository = new Repository(session);
                repository.Create(new Book
                {
                    Title = "Book with Review",
                    Language = "Venda",
                    Genre = "Fiction",
                    AgeRange = "0-2",
                    Posts = {new BookPost()
                        {
                            Title = "a Review",
                            Content = "A review",
                            Type = BookPost.BookPostType.Reviews
                        }
                    }
                });

                repository.Create(new Book
                {
                    Title = "Book without post",
                    Language = "Venda",
                    Genre = "Fiction",
                    AgeRange = "0-2"
                });

                repository.Create(new Book
                {
                    Title = "Book with Events",
                    Language = "Zulu",
                    Genre = "Fiction",
                    AgeRange = "0-2",
                    Posts = {new BookPost()
                        {
                            Title = "an Event",
                            Content = "An Event",
                            Type = BookPost.BookPostType.Events
                        }
                    }
                });
                session.SaveChanges();

                var booksController = new BooksController(repository);

                var view = (ViewResult)booksController.Filter(new List<string>() { "Venda" }, new List<string>(), new List<string>(), new List<string>() { "Reviews" });

                var filterInformation = (FilterInformation)view.Model;
                var actualBooks = filterInformation.BookInformations.Select(bookInformation => bookInformation.Model).ToList();
                Assert.AreEqual(1, actualBooks.Count());
                Assert.AreEqual("Book with Review", actualBooks.First().Title);
            }
        }
    }
}
