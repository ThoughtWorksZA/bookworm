using System.Web.Mvc;
using System.Web.UI.WebControls;
using BookWorm.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;

namespace BookWorm.Tests.Controllers
{
    [TestClass]
    public class AccountControllerTest
    {
        private class TestAccountController : AccountController
        {
            private readonly int _userCount;

            public TestAccountController(int userCount)
            {
                _userCount = userCount;
            }

            protected override int GetUsersCount()
            {
                return _userCount;
            }
        }

        [TestMethod]
        public void ShouldRenderRegisterPageWhenThereIsNoUser()
        {
            var controller = new TestAccountController(0);
            var result = (ViewResult)controller.Register();
            Assert.AreEqual("Register", result.ViewName);
        }

        [TestMethod]
        public void ShouldRedirectToErrorPageWhenThereAreExistingUsers()
        {
            var controller = new TestAccountController(1);
            var result = (HttpStatusCodeResult) controller.Register();
            Assert.AreEqual(403, result.StatusCode);
        }

        [TestMethod]
        public void ShouldSetReturnUrlOnLogin()
        {
            var accountController = new AccountController();
            accountController.Login("someUrl");
            string returnUrl = accountController.ViewBag.ReturnUrl;
            returnUrl.Should().Be("someUrl");
        }

        [TestMethod]
        public void ShouldReturnAViewOnLogin()
        {
            var accountController = new AccountController();
            var result = accountController.Login("someUrl");
            result.Should().BeOfType<ViewResult>();
        }
    }
}
