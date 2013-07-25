using System;
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
                    UserName = "Akani",
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