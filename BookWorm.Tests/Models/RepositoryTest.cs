using BookWorm.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Raven.Client;

namespace BookWorm.Tests.Models
{
    [TestClass]
    public class RepositoryTest
    {
        [TestMethod]
        public void ShouldKnowThatCreatedModelIsAssignedAnId()
        {
            var testModel = new StaticPage();
            var documentSession = new Mock<IDocumentSession>();

            documentSession.Setup(session => session.Store(testModel)).Callback<object>(arg => testModel.Id = 1);

            var repository = new Repository(documentSession.Object);
            repository.Create(testModel);

            documentSession.Verify(session => session.Store(testModel), Times.Once());
            Assert.AreNotEqual(0, testModel.Id);
        }

        [TestMethod]
        public void ShouldKnowHowToGetModelFromTheirId()
        {
            var persistedModel = new StaticPage {Title = "Nandos Rocks", Id = 1337, Content = "Nandos makes chicken. You're going to love it."};
            var documentSession = new Mock<IDocumentSession>();

            documentSession.Setup(session => session.Load<StaticPage>(persistedModel.Id)).Returns(persistedModel);
            var repository = new Repository(documentSession.Object);
            var retrievedModel = repository.Get<StaticPage>(persistedModel.Id);

            documentSession.Verify(session=>session.Load<StaticPage>(persistedModel.Id), Times.Once());
            Assert.AreEqual(persistedModel.Id , retrievedModel.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(Raven.Client.Exceptions.NonUniqueObjectException))]
        public void ShouldRethrowRavenNonUniqueObjectException()
        {
            var testModel = new StaticPage();
            var documentSession = new Mock<IDocumentSession>();

            documentSession.Setup(session => session.Store(testModel))
                           .Throws(new Raven.Client.Exceptions.NonUniqueObjectException());

            var repository = new Repository(documentSession.Object);
            repository.Create(testModel);
        }

        [TestMethod]
        public void ShouldKnowHowToDeleteAModelByItsIdWhenItExists()
        {

            var persistedModel = new StaticPage { Title = "Nandos Rocks", Id = 1337, Content = "Nandos makes chicken. You're going to love it." };
            var documentSession = new Mock<IDocumentSession>();
            documentSession.Setup(session => session.Load<StaticPage>(persistedModel.Id)).Returns(persistedModel);
            var repository = new Repository(documentSession.Object);

            repository.Delete<StaticPage>(persistedModel.Id);
            documentSession.Verify(session => session.Delete(persistedModel), Times.Once()); 
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void DeletingNonExistentDocumentShouldRethrowInvalidOperationException()
        {
            var persistedModel = new StaticPage { Title = "Nandos Rocks", Id = 1337, Content = "Nandos makes chicken. You're going to love it." };
            var documentSession = new Mock<IDocumentSession>();
            documentSession.Setup(session => session.Load<StaticPage>(persistedModel.Id)).Returns(persistedModel);
            documentSession.Setup(session => session.Delete(persistedModel))
                           .Throws(new System.InvalidOperationException());

            var repository = new Repository(documentSession.Object);
            repository.Delete<StaticPage>(persistedModel.Id);
        }

        [TestMethod]
        public void ShouldKnowHowToEditAModelByItsIdWhenItExists()
        {
            var persistedModel = new StaticPage { Title = "Nandos Rocks", Id = 1337, Content = "Nandos makes chicken. You're going to love it." };
            var documentSession = new Mock<IDocumentSession>();
            documentSession.Setup(session => session.Load<StaticPage>(persistedModel.Id)).Returns(persistedModel);
            var repository = new Repository(documentSession.Object);

            repository.Edit<StaticPage>(persistedModel);

            documentSession.Verify(session => session.Store(persistedModel), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void EditingNonExistentDocumentShouldRethrowInvalidOperationException()
        {
            var persistedModel = new StaticPage { Title = "Nandos Rocks", Id = 1337, Content = "Nandos makes veggie burger. You're going to love it." };
            var documentSession = new Mock<IDocumentSession>();
            documentSession.Setup(session => session.Load<StaticPage>(persistedModel.Id)).Returns(persistedModel);
            documentSession.Setup(session => session.Store(persistedModel))
                           .Throws(new System.InvalidOperationException());

            var repository = new Repository(documentSession.Object);
            repository.Edit<StaticPage>(persistedModel);
        }

        [TestMethod]
        public void ShouldKnowHowToDetatchDocumentFromSession()
        {
            var persistedModel = new StaticPage { Title = "Nandos Rocks", Id = 1337, Content = "Nandos makes chicken. You're going to love it." };
            var documentSession = new Mock<IDocumentSession>();
            var _syncAdvancedSessionOperation = new Mock<ISyncAdvancedSessionOperation>();
            documentSession.SetupGet(session => session.Advanced).Returns(_syncAdvancedSessionOperation.Object);
            var repository = new Repository(documentSession.Object);
            repository.Detach(persistedModel);
            _syncAdvancedSessionOperation.Verify(session => session.Evict(persistedModel), Times.Once());
        }
    }
}
