using System;
using System.Collections.Generic;
using System.Linq;
using BirdBrain;
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
        public void ShouldReturnTrueWhenDocumentCollectionHasAnyDocuments()
        {
            using (var session = _documentStore.OpenSession())
            {
                var user = new User {Username = "test"};
                session.Store(user);
                session.SaveChanges();
                var repository = new Repository(session);
                Assert.IsTrue(repository.Any<User>());
            }
        }

        [TestMethod]
        public void ShouldReturnFalseWhenDocumentCollectionIsEmpty()
        {
            using (var session = _documentStore.OpenSession())
            {
                var repository = new Repository(session);
                Assert.IsFalse(repository.Any<User>());
            }
        }
    }
}