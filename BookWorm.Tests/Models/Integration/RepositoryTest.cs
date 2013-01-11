using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using BookWorm.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raven.Client;
using Raven.Client.Embedded;

namespace BookWorm.Tests.Models.Integration
{
    [TestClass]
    public class RepositoryTest
    {
        private IDocumentStore _documentStore;

        [TestInitialize]
        public void Setup()
        {
            _documentStore = new EmbeddableDocumentStore
                {
                    RunInMemory = true
                };
            _documentStore.Initialize();
        }

        public ICollection<Book> CreateTenBooks()
        {
            ICollection<Book> books = new List<Book>();

            using (var session = _documentStore.OpenSession())
            {
                for (var i = 0; i < 10; i++)
                {
                    var book = new Book {Title = "Book " + i, Isbn = "12345678" + i};
                    books.Add(book);
                    session.Store(book);
                }
                session.SaveChanges();
            }
            return books;
        }

        [TestMethod]
        public void ListShouldReturnAllPersistedDocumentsOfTheSpecifiedType()
        {
            var books = CreateTenBooks();

            using (var session = _documentStore.OpenSession())
            {
                var repo = new Repository(session);
                var retrievedBooks = repo.List<Book>();

                Assert.AreEqual(books.Count, retrievedBooks.Count);
            }
        }

        [TestMethod]
        public void SearchShouldReturnDocumentsMatchingThePredicateWithTitle()
        {
            CreateTenBooks();
            using (var session = _documentStore.OpenSession())
            {
                var repo = new Repository(session);
                var retrievedBooks = repo.Search<Book>(book => book.Title == "Book 1" || book.Isbn == "Book 1");
                Assert.AreEqual(1, retrievedBooks.Count());
                Assert.AreEqual("Book 1", retrievedBooks.First().Title);
            }
        }

        [TestMethod]
        public void SearchShouldReturnDocumentsMatchingThePredicateWithIsbn()
        {
            CreateTenBooks();
            using (var session = _documentStore.OpenSession())
            {
                var repo = new Repository(session);
                var retrievedBooks = repo.Search<Book>(book => book.Title == "123456781" || book.Isbn == "123456781");
                Assert.AreEqual(1, retrievedBooks.Count());
                Assert.AreEqual("123456781", retrievedBooks.First().Isbn);
            }
        }

        [TestMethod]
        public void ShouldKnowHowToTakeNDocuments()
        {
            using (var session = _documentStore.OpenSession())
            {
                session.Store(new StaticPage { Title = "test" });
                session.Store(new StaticPage { Title = "test2" });
                session.SaveChanges();
                var repository = new Repository(session);
                Assert.AreEqual(1, repository.List<StaticPage>(1, 1).Count());
            }
        }

        [TestMethod]
        public void ShouldSetCreatedAtAndUpdatedAtOnCreate()
        {
            var before = DateTime.Now;
            using (var session = _documentStore.OpenSession())
            {
                var page = new StaticPage {Title = "test"};
                var repository = new Repository(session);
                var createdPage = repository.Create(page);
                Assert.IsTrue(before <= createdPage.CreatedAt);
                Assert.AreEqual(createdPage.CreatedAt, createdPage.UpdatedAt);
            }
        }

        [TestMethod]
        public void ShouldOnlySetUpdatedAtOnUpdate()
        {
            using (var session = _documentStore.OpenSession())
            {
                var page = new StaticPage { Title = "test" };
                var repository = new Repository(session);
                var createdPage = repository.Create(page);
                var created_at = createdPage.CreatedAt;
                createdPage.Title = "new title";
                var before = DateTime.Now;
                Thread.Sleep(100);
                repository.Edit(createdPage);
                Assert.IsTrue(before <= createdPage.UpdatedAt);
                Assert.AreEqual(created_at, createdPage.CreatedAt);
            }
        }

        [TestMethod]
        public void ShouldKnowListOrdersByCreatedAt()
        {
            using (var session = _documentStore.OpenSession())
            {
                session.Store(new StaticPage { Title = "test2", CreatedAt = DateTime.Now.AddMinutes(1) });
                session.Store(new StaticPage { Title = "test", CreatedAt = DateTime.Now });
                session.SaveChanges();
                var repository = new Repository(session);
                Assert.AreEqual("test", repository.List<StaticPage>(1).First().Title);
            }
        }

        [TestMethod]
        public void ShouldListSecondPage()
        {
            using (var session = _documentStore.OpenSession())
            {
                session.Store(new StaticPage { Title = "test2", CreatedAt = DateTime.Now.AddMinutes(1) });
                session.Store(new StaticPage { Title = "test", CreatedAt = DateTime.Now });
                session.SaveChanges();
                var repository = new Repository(session);
                Assert.AreEqual("test2", repository.List<StaticPage>(2, 1).First().Title);
            }
        }
    }
}