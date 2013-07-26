using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using BirdBrain;
using BookWorm.Controllers;
using BookWorm.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raven.Abstractions.Extensions;
using Raven.Client;
using Raven.Client.Document;

namespace BookWorm.Tests.Controllers.Integration
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
            _documentStore = new DocumentStore()
                {
                    ConnectionStringName = "RavenDB"
                };
            _documentStore.Initialize();
            Clean();
        }

        private void Clean()
        {
            using (var session = _documentStore.OpenSession())
            {
                var users = session.Query<User>();
                users.ForEach(u => session.Delete(u));
                session.SaveChanges();
            }
        }

        [TestMethod]
        public void ShouldGoToListPage()
        {
            var controller = new AccountController();
            var result = controller.List();
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ShouldListUsers()
        {
            var ruimin = new User()
            {
                Username = "Ruimin@tw.com",
                Roles = new string[] { Roles.Admin },
                Created = DateTime.UtcNow.AddDays(-1),
                IsApproved = true
            };

            var akani = new User()
            {
                Username = "Akani@tw.com",
                Roles = new string[] { Roles.Author },
                Created = DateTime.UtcNow,
                IsApproved = false
            };
            UsingSession(session =>
                {
                    session.Store(ruimin);
                    session.Store(akani);
                });

            UsingSession(session =>
                {
                    var accountController = new AccountController(session);
                    var viewResult = accountController.List();
                    var actualUsers = ((List<User>) (viewResult.Model));
                    Assert.AreEqual(2, actualUsers.Count);
                    Assert.IsTrue(UserEqual(actualUsers.First(), akani));
                    Assert.IsTrue(UserEqual(actualUsers.Last(), ruimin));
                });
        }

        private bool UserEqual(User expected, User actual)
        {
            return expected.Username == actual.Username && expected.Created == actual.Created &&
                   expected.Roles[0] == actual.Roles[0] && expected.IsApproved == actual.IsApproved;
        }

        [TestMethod]
        public void ShouldGoToNewAccountPage()
        {
            var controller = new AccountController();
            var result = controller.Create();
            Assert.AreEqual("Add a User", result.ViewBag.Title);
        }

        [TestMethod]
        public void ShouldCreateNewUser()
        {
            var model = new RegisterModel()
                {
                    UserName = "Akani@thoughtworks.com",
                    Password = "111111",
                    ConfirmPassword = "111111",
                    Role = Roles.Admin
                };

            UsingSession((session) =>
                {
                    var controller = new AccountController(session);

                    var actionResult = (System.Web.Mvc.RedirectToRouteResult)(controller.Create(model));
                    Assert.AreEqual("Account",actionResult.RouteValues["controller"]);
                    Assert.AreEqual("List",actionResult.RouteValues["action"]);
                });

            UsingSession((session) =>
                {
                    var users = session.Query<User>().Customize(a => a.WaitForNonStaleResultsAsOfLastWrite()).ToList();
                    Assert.AreEqual(1, users.Count());
                    var user = users.First();
                    Assert.AreEqual(model.UserName, user.Username);
                    Assert.AreEqual(model.Role, user.Roles.First());
                });
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