using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using BirdBrain;
using BookWorm.Controllers;
using BookWorm.Models;
using BookWorm.Services.Account;
using BookWorm.Services.Email;
using BookWorm.Tests.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Raven.Abstractions.Extensions;
using Raven.Client;
using Raven.Client.Document;

namespace BookWorm.Tests.Integration.Controllers
{
    [TestClass]
    public class AccountControllerTest
    {
        private IDocumentStore _documentStore;

        [TestInitialize]
        public void Setup()
        {
            if (ConfigurationManager.AppSettings["Environment"] == "Test")
                Assert.Inconclusive("Skipping test on AppHarbor");
            _documentStore = new DocumentStore
                {
                    ConnectionStringName = "RavenDB"
                };
            _documentStore.Initialize();
            DeleteUsers();
        }

        private void DeleteUsers()
        {
            using (var session = _documentStore.OpenSession())
            {
                var users = session.Query<User>();
                users.ForEach(u => session.Delete<User>(u));
                session.SaveChanges();
            }
        }

        [TestMethod]
        public void ShouldGoToListPage()
        {
            UsingSession(s =>
                {
                    var controller = new AccountController(s);
                    var result = controller.List();
                    Assert.IsNotNull(result);
                });
        }

        [TestMethod]
        public void ShouldListUsers()
        {
            var ruimin = new RegisterModelBuilder()
                .WithEmail("Ruimin@tw.com")
                .WithRole(Roles.Author)
                .Build();
            var akani = new RegisterModelBuilder()
                .WithEmail("Akani@tw.com")
                .WithRole(Roles.Admin)
                .Build();

            UsingSession(session =>
                {
                    var accountController = new AccountController(session)
                        {
                            Email = GetEmailMock().Object
                        };
                    accountController.Create(ruimin);
                    accountController.Create(akani);
                });

            UsingSession(session =>
                {
                    var accountController = new AccountController(session);
                    var viewResult = accountController.List();
                    var actualUsers = ((List<User>) (viewResult.Model));
                    Assert.AreEqual(2, actualUsers.Count);
                    Assert.IsTrue(UserEqual(akani, actualUsers.First()));
                    Assert.IsTrue(UserEqual(ruimin, actualUsers.Last()));
                });
        }

        private bool UserEqual(RegisterModel expected, User actual)
        {
            return expected.Email == actual.Username &&
                   expected.Role == actual.Roles[0];
        }

        [TestMethod]
        public void ShouldGoToNewAccountPage()
        {
            var controller = new AccountController();
            var result = controller.Create();
            Assert.AreEqual("Add a User", result.ViewBag.Title);
            Assert.AreEqual(RegisterModel.DefaultPassword, ((UserInformation)(result.Model)).Model.Password);
            Assert.AreEqual(RegisterModel.DefaultPassword, ((UserInformation)(result.Model)).Model.ConfirmPassword);
        }

        [TestMethod]
        public void ShouldCreateNewUser()
        {
            var model = new RegisterModelBuilder()
                .WithEmail("Akani@tw.com")
                .WithRole(Roles.Admin)
                .Build();

            UsingSession(session =>
                {
                    var controller = new AccountController(session)
                        {
                            Email = GetEmailMock().Object
                        };

                    var actionResult = (System.Web.Mvc.RedirectToRouteResult) (controller.Create(model));
                    Assert.AreEqual("Account", actionResult.RouteValues["controller"]);
                    Assert.AreEqual("List", actionResult.RouteValues["action"]);
                });

            UsingSession(session =>
                {
                    var users = session.Query<User>().Customize(a => a.WaitForNonStaleResultsAsOfLastWrite()).ToList();
                    Assert.AreEqual(1, users.Count());
                    var user = users.First();
                    Assert.AreEqual(model.Email, user.Username);
                    Assert.AreEqual(model.Role, user.Roles.First());
                });
        }

        [TestMethod]
        public void ShouldNotBeAbleToCreateUserWithAnExistingEmail()
        {
            var model = new RegisterModelBuilder()
                .WithEmail("Akani@tw.com")
                .WithRole(Roles.Admin)
                .Build();

            UsingSession(session =>
            {
                var controller = new AccountController(session)
                {
                    Email = GetEmailMock().Object
                };

                controller.Create(model);
            });

            UsingSession(session =>
            {
                var controller = new AccountController(session)
                {
                    Email = GetEmailMock().Object
                };

                var viewResult = (System.Web.Mvc.ViewResult)(controller.Create(model));

                Assert.AreEqual("A user with this email already exists", controller.TempData["flashError"]);
                Assert.AreEqual(string.Empty, viewResult.MasterName);
                Assert.AreEqual("Add a User", viewResult.ViewBag.Title);
            });
        }

        [TestMethod]
        public void ShouldCreateNewUserNotApproved()
        {
            var model = new RegisterModelBuilder()
                .Build();

            var mock = GetEmailMock();

            UsingSession(session =>
            {
                var controller = new AccountController(session) {Email = mock.Object};
                controller.Create(model);
            });

            UsingSession(session =>
            {
                var user = session.Query<User>().Customize(a => a.WaitForNonStaleResultsAsOfLastWrite()).First();
                Assert.IsFalse(user.IsApproved);
                mock.Verify(e => e.SendConfirmation("donotreply@puku.co.za", model.Email, user.ConfirmationToken, user.Id), Times.Once());
            });
        }

        [TestMethod]
        public void ShouldRedirectToChangePasswordToConfirmNewUserWithToken()
        {
            var mock = GetEmailMock();
            var secureToken = Guid.NewGuid().ToString();
            const int userId = 1;
            UsingSession(session =>
            {
                var controller = new AccountController(session) { Email = mock.Object };
                var registerConfirmationView = controller.RegisterConfirmation(secureToken, userId);
                Assert.AreEqual("Confirm user account", registerConfirmationView.ViewBag.Title);
                Assert.AreEqual(secureToken, ((LocalPasswordModel)(registerConfirmationView.Model)).SecurityToken);
                Assert.AreEqual(userId, ((LocalPasswordModel)(registerConfirmationView.Model)).UserId);
                Assert.AreEqual(RegisterModel.DefaultPassword, ((LocalPasswordModel)(registerConfirmationView.Model)).OldPassword);
            });
        }

        [TestMethod]
        public void ShouldConfirmNewUserWithTokenAndNewPassword()
        {
            var model = new RegisterModelBuilder()
                .Build();

            var mock = GetEmailMock();
            string secureToken = null;

            mock.Setup(e => e.SendConfirmation(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
                .Callback<string, string, string, int>((from, to, token, userId) =>
                {
                    secureToken = token;
                });
            string oldPassword = "";
            UsingSession(session =>
            {
                var controller = new AccountController(session) { Email = mock.Object };
                controller.Create(model);
                oldPassword = session.Query<User>().First(u => u.Username == model.Email).Password;
            });

            UsingSession(session =>
            {
                var user = session.Query<User>().First();
                var controller = new AccountController(session) { Email = mock.Object };
                var localPasswordModel = new LocalPasswordModelBuilder()
                    .WithSecurityToken(secureToken)
                    .WithUserId(user.Id)
                    .Build();
                var mockAccountService = new Mock<IAccountService>();
                mockAccountService.Setup(s => s.Login(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>())).Returns(true);
                controller.AccountService = mockAccountService.Object;
                var actionResult = (System.Web.Mvc.RedirectToRouteResult)(controller.RegisterConfirmation(localPasswordModel));
                Assert.AreEqual("Home", actionResult.RouteValues["controller"]);
                Assert.AreEqual("Index", actionResult.RouteValues["action"]);
            });

            UsingSession(session =>
            {
                Assert.IsTrue(session.Query<User>().First(u=>u.Username==model.Email).IsApproved);
                Assert.AreNotEqual(oldPassword,
                    session.Query<User>().First(u => u.Username == model.Email).Password);
            });
        }

        private static Mock<IEmail> GetEmailMock()
        {
            var mock = new Mock<IEmail>();
            mock.Setup(e => e.SendConfirmation(It.IsAny<string>(), It.IsAny<string>(),  It.IsAny<string>(), It.IsAny<int>()));
            return mock;
        }

        public void UsingSession(Action<IDocumentSession> action)
        {
            using (var session = _documentStore.OpenSession())
            {
                action(session);
                session.SaveChanges();
            }
        }
    }
}
