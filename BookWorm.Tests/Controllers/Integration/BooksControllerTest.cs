using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using BookWorm.Controllers;
using BookWorm.Models;
using BookWorm.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Raven.Client;
using Raven.Client.Embedded;

namespace BookWorm.Tests.Controllers.Integration
{
    [TestClass]
    public class BooksControllerTest
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

                var view = (ViewResult)booksController.Filter(new List<string>() { "Zulu", "Xhosa" });

                var filterInformation = (FilterInformation)view.Model;
                var actualBooks = filterInformation.BookInformations.Select(bookInformation => bookInformation.Book).ToList();
                Assert.AreEqual(2, actualBooks.Count());
                Assert.AreEqual(expectedBooks.First().Language, actualBooks.First().Language);
                Assert.AreEqual(expectedBooks.Last().Language, actualBooks.Last().Language);
                Assert.AreEqual(2, filterInformation.Languages.Count());
                Assert.AreEqual("Zulu", filterInformation.Languages.First());
                Assert.AreEqual("Xhosa", filterInformation.Languages.Last());
            }
        }
    }
}
