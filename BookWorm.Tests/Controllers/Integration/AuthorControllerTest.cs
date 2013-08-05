using System;
using System.Collections.Generic;
using BookWorm.Controllers;
using BookWorm.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Raven.Client;

namespace BookWorm.Tests.Controllers.Integration
{
    [TestClass]
    public class AuthorControllerTest : ControllerInMemoryRavenTestBase
    {
        [TestMethod]
        public void ShouldCreateNewAuthor()
        {
            var model = new Author()
                {
                    Name = "Author",
                    Biography = "Biography",
                    PictureUrl = "myPicture.jpg"
                };

            UsingSession((session) =>
                {
                    var controller = new AuthorController(new Repository(session));

                    var actionResult = (System.Web.Mvc.RedirectToRouteResult) (controller.Create(model));
                    Assert.AreEqual("Author", actionResult.RouteValues["controller"]);
                    Assert.AreEqual("List", actionResult.RouteValues["action"]);
                });

            UsingSession((session) =>
                {
                    var author = WaitForTheLastWrite<Author>(session);
                    AssertEqual(model, author);
                });
        }

        [TestMethod]
        public void ShouldNotCreateExistingAuthor()
        {
            var model = new Author()
                {
                    Name = "Author",
                    Biography = "Biography",
                    PictureUrl = "myPicture.jpg"
                };

            UsingSession((session) =>
                {
                    var controller = new AuthorController(new Repository(session));
                    controller.Create(model);
                });

            UsingSession((session) =>
                {
                    WaitForTheLastWrite<Author>(session);
                    var controller = new AuthorController(new Repository(session));
                    var viewResult = (System.Web.Mvc.ViewResult) (controller.Create(model));

                    Assert.AreEqual("An author with this name already exists", controller.TempData["flashError"]);

                    Assert.AreEqual("", viewResult.MasterName);
                    Assert.AreEqual("Create Author", viewResult.ViewBag.Title);
                });
        }

        [TestMethod]
        public void ShouldListAuthors()
        {
            var author1 = new Author()
                {
                    Name = "Author1",
                    Biography = "Biography1",
                    PictureUrl = "myPicture1.jpg",
                    CreatedAt = DateTime.UtcNow
                };

            var author2 = new Author()
                {
                    Name = "Author2",
                    Biography = "Biography2",
                    PictureUrl = "myPicture2.jpg",
                    CreatedAt = DateTime.UtcNow.AddDays(-1)
                };

            UsingSession((session) =>
                {
                    var controller = new AuthorController(new Repository(session));
                    controller.Create(author1);
                    controller.Create(author2);
                });

            UsingSession((session) =>
                {
                    WaitForTheLastWrite<Author>(session);
                    var controller = new AuthorController(new Repository(session));
                    var viewResult = controller.List();
                    var authors = (List<Author>) (viewResult.Model);

                    Assert.AreEqual("", viewResult.MasterName);
                    Assert.AreEqual("Authors", viewResult.ViewBag.Title);

                    Assert.AreEqual(2, authors.Count);
                    AssertEqual(author1, authors.ElementAt(0));
                    AssertEqual(author2, authors.ElementAt(1));
                });
        }

        private void AssertEqual(Author expected, Author actual)
        {
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.PictureUrl, actual.PictureUrl);
            Assert.AreEqual(expected.Biography, actual.Biography);
        }
    }
}