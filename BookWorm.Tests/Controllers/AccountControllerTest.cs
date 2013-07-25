using System.Web.Mvc;
using BookWorm.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
    }
}
