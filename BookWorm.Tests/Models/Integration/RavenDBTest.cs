using System.Linq;
using System.Threading;
using BookWorm.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Embedded;
using Raven.Client.Indexes;

namespace BookWorm.Tests.Models.Integration
{
    // Integration tests that ensure our assumptions about Raven are valid.
    [TestClass]
    public class RavenDBTest
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
        public void ShouldKnowThatModelsGetAnIdOnStore()
        {
            var session = _documentStore.OpenSession();
            var model = new Model();
            session.Store(model);
            Assert.AreNotEqual(0, model.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(Raven.Client.Exceptions.NonUniqueObjectException))]
        public void ShouldKnowThatWeCannotOverwriteADocumentWithAnotherDocument()
        {
            var session = _documentStore.OpenSession();
            var model = new Model { Id = 1 };
            var anotherModel = new Model { Id = 1 };
            session.Store(model);
            session.Store(anotherModel);
        }

        [TestMethod]
        public void ShouldKnowThatWeCanOverwriteADocumentWithItself()
        {
            var session = _documentStore.OpenSession();
            var model = new StaticPage { Id = 1, Title = "Misspelled" };
            session.Store(model);
            model.Title = "Spelled";
            session.Store(model);
        }

        [TestMethod]
        public void ShouldKnowThatWeCanDeleteADocument()
        {
            var session = _documentStore.OpenSession();
            var model = new StaticPage { Id = 1, Title = "Misspelled" };
            session.Store(model);
            session.SaveChanges();
            session.Delete(model);
            session.SaveChanges();
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void ShouldKnowThatWeCannotDeleteADocumentTwice()
        {
            var session = _documentStore.OpenSession();
            var model = new StaticPage { Id = 1, Title = "Misspelled" };
            session.Store(model);
            session.Delete(model);
            session.SaveChanges();
            session.Delete(model);
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void ShouldKnowThatWeCannotDeleteAnInvalidDocument()
        {
            var session = _documentStore.OpenSession();
            var model = new StaticPage { Id = 1, Title = "Misspelled" };
            session.Delete(model);
        }

        [TestMethod]
        public void ShouldKnowWeCanEvict()
        {
            var session = _documentStore.OpenSession();
            var model = new StaticPage { Id = 1, Title = "Misspelled" };
            session.Advanced.Evict(model);
        }
    }
}
