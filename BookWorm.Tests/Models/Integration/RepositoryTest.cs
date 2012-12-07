using BookWorm.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raven.Client;
using Raven.Client.Embedded;

namespace BookWorm.Tests.Models.Integration
{
    // Integration tests that ensure our assumptions about Raven are valid.
    [TestClass]
    public class RepositoryTest
    {
        [TestMethod]
        public void ShouldKnowThatModelsGetAnIdOnStore()
        {
            IDocumentStore documentStore = new EmbeddableDocumentStore
            {
                RunInMemory = true
            };

            documentStore.Initialize();
            var session = documentStore.OpenSession();
            var model = new Model<object>();
            session.Store(model);
            Assert.AreNotEqual(0, model.Id);
        }
    }
}
