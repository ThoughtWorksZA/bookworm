using System;
using System.Web.Mvc;
using BookWorm.Controllers;
using BookWorm.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using PagedList;

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
            RedirectToRouteResult actionResult = null;
            UsingSession((session) =>
                {
                    var controller = new AuthorController(new Repository(session));

                    actionResult = (RedirectToRouteResult) (controller.Create(model));
                    Assert.AreEqual("Author", actionResult.RouteValues["controller"]);
                    Assert.AreEqual("Details", actionResult.RouteValues["action"]);
                });

            UsingSession((session) =>
                {
                    var author = WaitForTheLastWrite<Author>(session);
                    Assert.AreEqual(author.Id, actionResult.RouteValues["id"]);
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
                    var authors = (IPagedList<Author>)(viewResult.Model);

                    Assert.AreEqual("", viewResult.MasterName);
                    Assert.AreEqual("Authors", viewResult.ViewBag.Title);

                    Assert.AreEqual(2, authors.Count);
                    AssertEqual(author1, authors.ElementAt(0));
                    AssertEqual(author2, authors.ElementAt(1));
                });
        }

        [TestMethod]
        public void ShouldRetrieveTheAuthorToEdit()
        {
            var author1 = new Author()
            {
                Name = "Author1",
                Biography = "Biography1",
                PictureUrl = "myPicture1.jpg",
                CreatedAt = DateTime.UtcNow
            };

            UsingSession((session) =>
            {
                var controller = new AuthorController(new Repository(session));
                controller.Create(author1);
            });

            UsingSession((session) =>
            {
                var author = WaitForTheLastWrite<Author>(session);
                var controller = new AuthorController(new Repository(session));
                var viewResult = controller.Edit(author.Id);
                var authorInView = (Author)(viewResult.Model);
                
                AssertEqual(author1,authorInView);
            });
        }

        [TestMethod]
        public void ShouldUpdateAuthor()
        {
            var author1 = new Author()
            {
                Name = "Author1",
                Biography = "Biography1",
                PictureUrl = "myPicture1.jpg",
                CreatedAt = DateTime.UtcNow
            };

            UsingSession((session) =>
            {
                var controller = new AuthorController(new Repository(session));
                controller.Create(author1);
            });

            Author updatedAuthorInfo = null;
            UsingSession((session) =>
            {
                var author = WaitForTheLastWrite<Author>(session);

                updatedAuthorInfo = new Author()
                {
                    Id = author.Id,
                    Name = "Author updated",
                    Biography = "Biography updated",
                    PictureUrl = "myPictureupdated.jpg",
                };
                var controller = new AuthorController(new Repository(session));
                var actionResult = (RedirectToRouteResult)(controller.Edit(updatedAuthorInfo));
                Assert.AreEqual("Author", actionResult.RouteValues["controller"]);
                Assert.AreEqual("Details", actionResult.RouteValues["action"]);
                Assert.AreEqual(author.Id, actionResult.RouteValues["id"]);
            });

            UsingSession((session) =>
            {
                var updatedAuthor = WaitForTheLastWrite<Author>(session);
                AssertEqual(updatedAuthorInfo, updatedAuthor);
            });
        }

        [TestMethod]
        public void ShouldListAuthorsWithPagination()
        {
            UsingSession((session) =>
                {
                    var controller = new AuthorController(new Repository(session));
                    Enumerable.Range(1, 9)
                              .ToList()
                              .ForEach(i => controller.Create(new Author {Name = "Author " + i, Biography = "Biography " + i}));
                });

            UsingSession((session) =>
            {
                WaitForTheLastWrite<Author>(session);
                var controller = new AuthorController(new Repository(session));
                var viewResult = controller.List(1, 4);
                var authors = (IPagedList<Author>)(viewResult.Model);
                Assert.AreEqual(4, authors.Count);
                Assert.AreEqual(3, authors.PageCount);
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