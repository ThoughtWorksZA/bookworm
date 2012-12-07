using System;
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
    }
}
