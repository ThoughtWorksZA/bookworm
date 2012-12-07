using System;
using System.Web.Mvc;
using BookWorm.Controllers;
using BookWorm.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Raven.Client;

namespace BookWorm.Tests.Controllers
{
    internal class TestBaseController : BaseController
    {
        private readonly IDocumentStore _documentStore;

        public TestBaseController(IDocumentStore documentStore)
        {
            _documentStore = documentStore;
        }

        protected override IDocumentStore GetDocumentStore()
        {
            return _documentStore;
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
    }
}
