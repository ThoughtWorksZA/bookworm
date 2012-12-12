using System;
using System.Collections.Generic;
using System.Web.Mvc;
using BookWorm.Controllers;
using BookWorm.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Raven.Client;
using Raven.Client.Linq;

namespace BookWorm.Tests.Controllers
{
    internal class TestRepository : Repository
    {
        public TestRepository(IDocumentSession documentSession) : base(documentSession)
        {
        }

        public override ICollection<T> List<T>()
        {
            return new List<T>();
        }
    }

    internal class TestBaseController : BaseController
    {
        private IDocumentStore _documentStore;
        private Repository _repository;

        public TestBaseController(IDocumentStore documentStore)
        {
            _documentStore = documentStore;
        }

        public TestBaseController(Repository repository) : base(repository)
        {
        }

        protected override IDocumentStore GetDocumentStore()
        {
            if (_documentStore == null)
            {
                var documentSession = new Mock<IDocumentSession>();
                var documentStore = new Mock<IDocumentStore>();
                documentStore.Setup(store => store.OpenSession()).Returns(documentSession.Object);
                _documentStore = documentStore.Object;
            }
            return _documentStore;
        }

        protected override Repository GetRepository()
        {
            if (_repository == null)
            {
                _session = GetDocumentStore().OpenSession();
                _repository = new TestRepository(_session);
            }
            return _repository;
        }

        public new void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
        }

        public new void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
        }
    }

    [TestClass]
    public class BaseControllerTest
    {
        [TestMethod]
        public void ShouldKnowToOpenSessionOnActionExecuting()
        {
            var documentSession = new Mock<IDocumentSession>();
            documentSession.Setup(session => session.SaveChanges());
            var documentStore = new Mock<IDocumentStore>();
            documentStore.Setup(store => store.OpenSession()).Returns(documentSession.Object);
            var testBaseController = new TestBaseController(documentStore.Object);
            testBaseController.OnActionExecuting(null);
            documentStore.Verify(store => store.OpenSession(), Times.Once());
        }

        [TestMethod]
        public void ShouldKnowToSaveChangesOnActionExecuted()
        {
            var documentSession = new Mock<IDocumentSession>();
            var documentStore = new Mock<IDocumentStore>();
            documentStore.Setup(store => store.OpenSession()).Returns(documentSession.Object);

            var testBaseController = new TestBaseController(documentStore.Object);
            testBaseController.OnActionExecuting(null);
            
            var actionExecutedContext = new Mock<ActionExecutedContext>();
            actionExecutedContext.SetupGet(context=>context.IsChildAction).Returns(false);
            actionExecutedContext.Object.Exception = null;

            testBaseController.OnActionExecuted(actionExecutedContext.Object);

            documentSession.Verify(session=>session.SaveChanges(), Times.Once());
        }

        [TestMethod]
        public void ShouldKnowToNotSaveChangesOnActionExecutedWhenRunOnChildAction()
        {
            var documentSession = new Mock<IDocumentSession>();
            documentSession.Setup(session => session.SaveChanges());
            var documentStore = new Mock<IDocumentStore>();
            documentStore.Setup(store => store.OpenSession()).Returns(documentSession.Object);

            var testBaseController = new TestBaseController(documentStore.Object);
            testBaseController.OnActionExecuting(null);

            var actionExecutedContext = new Mock<ActionExecutedContext>();
            actionExecutedContext.SetupGet(context => context.IsChildAction).Returns(true);
            actionExecutedContext.Object.Exception = null;

            testBaseController.OnActionExecuted(actionExecutedContext.Object);

            documentSession.Verify(session => session.SaveChanges(), Times.Never());
        }

        [TestMethod]
        public void ShouldKnowToNotSaveChangesOnActionExecutedWhenExceptionPresentInContext()
        {
            var documentSession = new Mock<IDocumentSession>();
            documentSession.Setup(session => session.SaveChanges());
            var documentStore = new Mock<IDocumentStore>();
            documentStore.Setup(store => store.OpenSession()).Returns(documentSession.Object);

            var testBaseController = new TestBaseController(documentStore.Object);
            testBaseController.OnActionExecuting(null);

            var actionExecutedContext = new Mock<ActionExecutedContext>();
            actionExecutedContext.SetupGet(context => context.IsChildAction).Returns(false);
            actionExecutedContext.Setup(context => context.Exception).Returns(new Exception("test exception"));

            testBaseController.OnActionExecuted(actionExecutedContext.Object);

            documentSession.Verify(session => session.SaveChanges(), Times.Never());
        }

        [TestMethod]
        [Ignore]
        public void ShouldKnowHowToAddStaticPagesToTheViewBag()
        {
            var repository = new Mock<Repository>();
            var savedPages = new List<StaticPage> 
                {
                    new StaticPage { Id = 1, Title = "test title", Content = "Hello\n=====\nWorld" }, 
                    new StaticPage { Id = 2, Title = "test title2", Content = "Hello\n=====\nAnother World" }
                };
            repository.Setup(repo => repo.List<StaticPage>()).Returns(savedPages);
            var controller = new TestBaseController(repository.Object);
            repository.Verify(repo => repo.List<StaticPage>(), Times.Never());
            controller.OnActionExecuting(null);
            Assert.AreEqual(savedPages.ToArray(), controller.ViewBag.StaticPages.ToArray());
            repository.Verify(repo => repo.List<StaticPage>(), Times.Once());
        }
    }
}
