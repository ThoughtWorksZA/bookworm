using System;
using System.Collections.Generic;
using System.Linq;
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
                    var book = new Book {Title = "Book " + i};
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
    }
}