using System;
using System.Web.Mvc;
using BirdBrain;
using BookWorm.Controllers;
using BookWorm.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BookWorm.Tests.Controllers
{
    [TestClass]
    public class AccountControllerTest
    {
        [TestMethod]
        public void ShouldRenderRegisterPageWhenThereIsNoUser()
        {
            var repository = new Mock<Repository>();
            var controller = new AccountController(repository.Object);
            var result = (ViewResult)controller.Register();
            Assert.AreEqual("Register", result.ViewName);
        }

        [TestMethod]
        public void ShouldRedirectToErrorPageWhenThereAreExistingUsers()
        {
            var repository = new Mock<Repository>();
            repository.Setup(repo => repo.Any<User>()).Returns(true);
            var controller = new AccountController(repository.Object);
            var result = (HttpStatusCodeResult) controller.Register();
            Assert.AreEqual(403, result.StatusCode);
        }
    }
}
