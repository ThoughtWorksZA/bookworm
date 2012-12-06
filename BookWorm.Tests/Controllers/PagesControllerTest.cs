using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using BookWorm.Controllers;
using BookWorm.Models;
using BookWorm.Repository;
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
            var repository = new Mock<Repository<StaticPage>>();
            var submittedPage = new StaticPage { Title = "test title", Content = "some content" };
            repository.Setup(repo => repo.Create(submittedPage)).Returns(new StaticPage { Id = 1, Title = submittedPage.Title, Content = submittedPage.Content });
            var controller = new PagesController(repository.Object);
            var result = (RedirectToRouteResult)controller.New(submittedPage);
            repository.Verify(it => it.Create(submittedPage), Times.Once());
            Assert.AreEqual(string.Format("Added {0}", submittedPage.Title), controller.TempData["flash"]);
            Assert.AreEqual(1, result.RouteValues["id"]);
        }

        [TestMethod]
        public void ShouldNotStorePageWhenTitleIsInvalid()
        {
            var repository = new Mock<Repository<StaticPage>>();
            var submittedPage = new StaticPage { Title = "", Content = "some content" };
            repository.Setup(repo => repo.Create(submittedPage)).Returns(submittedPage);
            
            var controller = new PagesController(repository.Object);
            ValidateViewModel(submittedPage, controller);
            Assert.IsFalse(controller.ModelState.IsValid);
        }

        [TestMethod]
        public void ShouldNotStorePageWhenContentIsInvalid()
        {
            var repository = new Mock<Repository<StaticPage>>();
            var submittedPage = new StaticPage { Title = "test title", Content = "" };
            repository.Setup(repo => repo.Create(submittedPage)).Returns(submittedPage);

            var controller = new PagesController(repository.Object);
            ValidateViewModel(submittedPage, controller);
            Assert.IsFalse(controller.ModelState.IsValid);
        }
    }
}
