using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using BookWorm.Controllers;
using BookWorm.Models;
using MarkdownSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BookWorm.Tests.Controllers
{
    [TestClass]
    public class PagesControllerTest
    {
        private static void ValidateViewModel<VM, C>(VM viewModelToValidate, C controller) where C : Controller
        {
            var validationContext = new ValidationContext(viewModelToValidate, null, null);
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(viewModelToValidate, validationContext, validationResults, true);
            foreach (var validationResult in validationResults)
            {
                controller.ModelState.AddModelError(validationResult.MemberNames.First(), validationResult.ErrorMessage);
            }
        }

        [TestMethod]
        public void ShouldStorePageWhenCreated()
        {
            var repository = new Mock<Repository>();
            var submittedPage = new StaticPage { Title = "test title", Content = "some content" };
            repository.Setup(repo => repo.Create(submittedPage)).Returns(new StaticPage { Id = 1, Title = submittedPage.Title, Content = submittedPage.Content });
            var controller = new PagesController(repository.Object);
            var result = (RedirectToRouteResult)controller.Create(submittedPage);
            repository.Verify(it => it.Create(submittedPage), Times.Once());
            Assert.AreEqual(string.Format("Added {0}", submittedPage.Title), controller.TempData["flashSuccess"]);
            Assert.AreEqual(1, result.RouteValues["id"]);
        }

        [TestMethod]
        public void ShouldNotStorePageWhenTitleIsInvalid()
        {
            var repository = new Mock<Repository>();
            var submittedPage = new StaticPage { Title = "", Content = "some content" };
            repository.Setup(repo => repo.Create(submittedPage)).Returns(submittedPage);
            
            var controller = new PagesController(repository.Object);
            ValidateViewModel(submittedPage, controller);
            Assert.IsFalse(controller.ModelState.IsValid);
        }

        [TestMethod]
        public void ShouldNotStorePageWhenContentIsInvalid()
        {
            var repository = new Mock<Repository>();
            var submittedPage = new StaticPage { Title = "test title", Content = "" };
            repository.Setup(repo => repo.Create(submittedPage)).Returns(submittedPage);

            var controller = new PagesController(repository.Object);
            ValidateViewModel(submittedPage, controller);
            Assert.IsFalse(controller.ModelState.IsValid);
        }

        [TestMethod]
        public void ShouldHandleDuplicatePageProblem()
        {
            var repository = new Mock<Repository>();
            var duplicatePage = new StaticPage { Title = "test title", Content = "test content" };
            repository.Setup(repo => repo.Create(duplicatePage))
                      .Throws(new Raven.Client.Exceptions.NonUniqueObjectException());

            var controller = new PagesController(repository.Object);
            controller.Create(duplicatePage);

            Assert.AreEqual(string.Format("Sorry, page {0} already exists.", duplicatePage.Title), controller.TempData["flashError"]);
        }

        [TestMethod]
        public void ShouldDisplayFormForNewPage()
        {
            var repository = new Mock<Repository>();
            var controller = new PagesController(repository.Object);
            var result = (ViewResult)controller.Create();
            Assert.AreEqual("Create", result.ViewName);
        }

        [TestMethod]
        public void ShouldKnowHowToDisplayAPage()
        {
            var id = 12;
            var repository = new Mock<Repository>();
            var savedPage = new StaticPage { Id = id, Title = "test title", Content = "some content" };
            repository.Setup(repo => repo.Get<StaticPage>(id)).Returns(savedPage);
            var controller = new PagesController(repository.Object);
            var result = controller.Details(id);
            repository.Verify(it => it.Get<StaticPage>(id), Times.Once());
            Assert.AreEqual(id, ((StaticPage)result.Model).Id);
        }

        [TestMethod]
        public void ShouldKnowToRenderThePageContentAsMarkdown()
        {
            var id = 12;
            var repository = new Mock<Repository>();
            var markdown = new Markdown();
            var savedPage = new StaticPage { Id = id, Title = "test title", Content = "Hello\n=====\nWorld" };
            repository.Setup(repo => repo.Get<StaticPage>(id)).Returns(savedPage);
            var transformedContent = markdown.Transform(savedPage.Content);
            var controller = new PagesController(repository.Object);
            var result = controller.Details(id);
            Assert.AreEqual(transformedContent, result.ViewBag.transformedContent);
        }

        [TestMethod]
        public void ShouldKnowHowToListAllPages()
        {
            var repository = new Mock<Repository>();
            var savedPages = new List<StaticPage> 
                {
                    new StaticPage { Id = 1, Title = "test title", Content = "Hello\n=====\nWorld" }, 
                    new StaticPage { Id = 2, Title = "test title2", Content = "Hello\n=====\nAnother World" }
                };
            repository.Setup(repo => repo.List<StaticPage>()).Returns(savedPages);
            var controller = new PagesController(repository.Object);
            var result = controller.List();
            repository.Verify(it => it.List<StaticPage>(), Times.Once());
            Assert.AreEqual(savedPages, result.Model);
        }

        [TestMethod]
        public void ShouldKnowHowToDeletePage()
        {
            var id = 1;
            var repository = new Mock<Repository>();
            var controller = new PagesController(repository.Object);
            var result = (RedirectToRouteResult)controller.Delete(id);
            repository.Verify(it => it.Delete<StaticPage>(id), Times.Once());
            Assert.AreEqual("List", result.RouteValues["action"]);
            Assert.AreEqual("Pages", result.RouteValues["controller"]);
        }

        [TestMethod]
        public void ShouldKnowHowToRenderAnEditPage()
        {
            var repositoryMock = new Mock<Repository>();
            var page = new StaticPage {Id = 1, Title = "The Page"};
            repositoryMock.Setup(repo => repo.Get<StaticPage>(page.Id)).Returns(page);
            var pagesController = new PagesController(repositoryMock.Object);

            var result = pagesController.Edit(page.Id);
            var actualModel = (StaticPage) result.Model;

            Assert.AreEqual(page.Title, actualModel.Title);
            repositoryMock.Verify(repo => repo.Get<StaticPage>(1), Times.Once());
        }

        [TestMethod]
        public void ShouldKnowHowToUpdateAPage()
        {
            var repository = new Mock<Repository>();
            var existingPage = new StaticPage {Id = 1, Title = "Derping for dummies."};
            repository.Setup(repo => repo.Edit(existingPage));
            var pagesController = new PagesController(repository.Object);
            var result = pagesController.Edit(existingPage);
            Assert.AreEqual(existingPage.Id, result.RouteValues["id"]);
            repository.Verify(repo => repo.Edit(existingPage), Times.Once());
        }

        [TestMethod]
        public void ShouldKnowPageControllerRequiresAuthorization()
        {
            var pagesControllerClass = typeof (PagesController);
            Assert.AreEqual(1, pagesControllerClass.GetCustomAttributes(typeof (AuthorizeAttribute), false).Count());
        }

        [TestMethod]
        public void ShouldKnowPageControllerListAllowsAnonymous()
        {
            var pagesControllerClass = typeof(PagesController);
            Assert.AreEqual(1, pagesControllerClass.GetMethods()
                                                   .First(method => method.Name == "List")
                                                   .GetCustomAttributes(typeof (AllowAnonymousAttribute), false)
                                                   .Count());
        }

        [TestMethod]
        public void ShouldKnowPageControllerDetailsAllowsAnonymous()
        {
            var pagesControllerClass = typeof(PagesController);
            Assert.AreEqual(1, pagesControllerClass.GetMethods()
                                                   .First(method => method.Name == "Details")
                                                   .GetCustomAttributes(typeof(AllowAnonymousAttribute), false)
                                                   .Count());
        }
    }
}
