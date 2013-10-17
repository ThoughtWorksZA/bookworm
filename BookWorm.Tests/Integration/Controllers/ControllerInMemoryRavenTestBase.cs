using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raven.Client;
using Raven.Client.Embedded;

namespace BookWorm.Tests.Integration.Controllers
{
    public class ControllerInMemoryRavenTestBase
    {
        protected IDocumentStore _documentStore;

        [TestInitialize]
        public void Setup()
        {
            _documentStore = new EmbeddableDocumentStore
                {
                    RunInMemory = true
                };
            _documentStore.Initialize();
        }

        protected void UsingSession(Action<IDocumentSession> action)
        {
            using (var session = _documentStore.OpenSession())
            {
                action(session);
                session.SaveChanges();
            }
        }

        protected static T WaitForTheLastWrite<T>(IDocumentSession session)
        {
            return session.Query<T>().Customize(a => a.WaitForNonStaleResultsAsOfLastWrite()).First();
        }
    }
}