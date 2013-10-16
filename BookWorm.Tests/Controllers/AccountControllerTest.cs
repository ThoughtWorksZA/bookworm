using System.IO;
using System.Security.Policy;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.UI;
using System.Web.UI.WebControls;
using BookWorm.Controllers;
using BookWorm.Models;
using BookWorm.Services.Account;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using Moq;
using Org.BouncyCastle.Asn1.Ocsp;

namespace BookWorm.Tests.Controllers
{
    [TestClass]
    public class AccountControllerTest
    {
        private class TestAccountController : AccountController
        {
            private readonly int _userCount;

            public TestAccountController(int userCount = 0)
            {
                _userCount = userCount;
            }

            protected override int GetUsersCount()
            {
                return _userCount;
            }

            protected override bool IsLocalUrl(string returnUrl)
            {
                return true;
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

        [TestMethod]
        public void ShouldLogUserInIfValid()
        {
            var accountController = new TestAccountController();
            var accountService = new Mock<AccountService>();
            accountController.AccountService = accountService.Object;
            var loginModel = new LoginModel{ Email = "email", Password = "password", RememberMe = true};
            accountService.Setup(it => it.Login("email", "password", true)).Returns(true);

            accountController.Login(loginModel, "someUrl");

            accountService.Verify(it => it.Login("email", "password", true));
        }
    }
}
