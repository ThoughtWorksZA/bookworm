using System.Web.Mvc;
using BookWorm.Controllers;
using BookWorm.Models;
using BookWorm.Services.Account;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using Moq;

namespace BookWorm.Tests.Controllers
{
    [TestClass]
    public class AccountControllerTest
    {
        private Mock<AccountService> _accountService;
        private TestAccountController _accountController = new TestAccountController();

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

        [TestInitialize]
        public void Setup()
        {
            _accountService = new Mock<AccountService>();
        }

        [TestMethod]
        public void ShouldRenderRegisterPageWhenThereIsNoUser()
        {
            var result = (ViewResult)_accountController.Register();
            result.ViewName.Should().Be("Register");
        }

        [TestMethod]
        public void ShouldRedirectToErrorPageWhenThereAreExistingUsers()
        {
            _accountController = new TestAccountController(1);
            var result = (HttpStatusCodeResult) _accountController.Register();
            result.StatusCode.Should().Be(403);
        }

        [TestMethod]
        public void ShouldSetReturnUrlOnLogin()
        {
            _accountController.Login("someUrl");
            string returnUrl = _accountController.ViewBag.ReturnUrl;
            returnUrl.Should().Be("someUrl");
        }

        [TestMethod]
        public void ShouldReturnAViewOnLogin()
        {
            var result = _accountController.Login("someUrl");
            result.Should().BeOfType<ViewResult>();
        }

        [TestMethod]
        public void ShouldLogInUserWhenModelStateIsValid()
        {
            _accountController.AccountService = _accountService.Object;
            var loginModel = new LoginModel{ Email = "email", Password = "password", RememberMe = true};
            _accountService.Setup(it => it.Login("email", "password", true)).Returns(true);

            _accountController.Login(loginModel, "someUrl");

            _accountService.Verify(it => it.Login("email", "password", true));
        }

        [TestMethod]
        public void ShouldAddErrorAndRedirectWhenLoginFails()
        {
            _accountController.AccountService = _accountService.Object;
            var loginModel = new LoginModel { Email = "email", Password = "password", RememberMe = true };
            _accountService.Setup(it => it.Login("email", "password", true)).Returns(false);

            var result = (ViewResult) _accountController.Login(loginModel, "someUrl");

            _accountController.ModelState.IsValid.Should().BeFalse("there should be an error added to the ModelState");
            result.Model.Should().Be(loginModel);
        }

        [TestMethod]
        public void ShouldAddErrorAndRedirectWhenModelIsInvalid()
        {
            var loginModel = new LoginModel { Email = "email", Password = "password", RememberMe = true };
            _accountService.Setup(it => it.Login("email", "password", true)).Returns(true);
            _accountController.ModelState.AddModelError("something", "Error");

            var result = _accountController.Login(loginModel, "someUrl");

            _accountController.ModelState.IsValid.Should().BeFalse("there should be an error added to the ModelState");
            result.Should().BeOfType<ViewResult>();
            ((ViewResult)result).Model.Should().Be(loginModel);
        }
    }
}
