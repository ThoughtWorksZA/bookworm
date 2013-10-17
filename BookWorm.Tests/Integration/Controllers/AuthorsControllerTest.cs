using System;
using System.Linq;
using System.Web.Mvc;
using BookWorm.Controllers;
using BookWorm.Models;
using BookWorm.Tests.Controllers;
using BookWorm.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PagedList;

namespace BookWorm.Tests.Integration.Controllers
{
    [TestClass]
    public class AuthorsControllerTest : ControllerInMemoryRavenTestBase
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
                    var controller = new AuthorsController(new Repository(session));

                    actionResult = (RedirectToRouteResult) (controller.Create(model));
                    Assert.AreEqual("Authors", actionResult.RouteValues["controller"]);
                    Assert.AreEqual("Details", actionResult.RouteValues["action"]);
                });

            UsingSession((session) =>
                {
                    var author = WaitForTheLastWrite<Author>(session);
                    Assert.AreEqual(author.Id, actionResult.RouteValues["id"]);
                    AuthorsContollerTestHelper.AssertEqual(model, author);
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
                    var controller = new AuthorsController(new Repository(session));
                    controller.Create(model);
                });

            UsingSession((session) =>
                {
                    WaitForTheLastWrite<Author>(session);
                    var controller = new AuthorsController(new Repository(session));
                    var viewResult = (System.Web.Mvc.ViewResult) (controller.Create(model));

                    Assert.AreEqual("An author with this name already exists", controller.TempData["flashError"]);

                    Assert.AreEqual("", viewResult.MasterName);
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
                    var controller = new AuthorsController(new Repository(session));
                    controller.Create(author1);
                    controller.Create(author2);
                });

            UsingSession((session) =>
                {
                    WaitForTheLastWrite<Author>(session);
                    var controller = new AuthorsController(new Repository(session));
                    var viewResult = controller.List();
                    var authors = (IPagedList<Author>)(viewResult.Model);

                    Assert.AreEqual("", viewResult.MasterName);
                    Assert.AreEqual("Authors", viewResult.ViewBag.Title);

                    Assert.AreEqual(2, authors.Count);
                    AuthorsContollerTestHelper.AssertEqual(author1, authors.ElementAt(0));
                    AuthorsContollerTestHelper.AssertEqual(author2, authors.ElementAt(1));
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
                var controller = new AuthorsController(new Repository(session));
                controller.Create(author1);
            });

            UsingSession((session) =>
            {
                var author = WaitForTheLastWrite<Author>(session);
                var controller = new AuthorsController(new Repository(session));
                var viewResult = controller.Edit(author.Id);
                var authorInView = (Author)(viewResult.Model);

                AuthorsContollerTestHelper.AssertEqual(author1, authorInView);
            });
        }

        [TestMethod]
        public void ShouldEditAuthor()
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
                var controller = new AuthorsController(new Repository(session));
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
                var controller = new AuthorsController(new Repository(session));
                var actionResult = (RedirectToRouteResult)(controller.Edit(updatedAuthorInfo));
                Assert.AreEqual("Authors", actionResult.RouteValues["controller"]);
                Assert.AreEqual("Details", actionResult.RouteValues["action"]);
                Assert.AreEqual(author.Id, actionResult.RouteValues["id"]);
            });

            UsingSession((session) =>
            {
                var updatedAuthor = WaitForTheLastWrite<Author>(session);
                AuthorsContollerTestHelper.AssertEqual(updatedAuthorInfo, updatedAuthor);
            });
        }

        [TestMethod]
        public void ShouldNotEditAuthorNameToAnExsitingOne()
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
                CreatedAt = DateTime.UtcNow
            };

            UsingSession((session) =>
            {
                var controller = new AuthorsController(new Repository(session));
                controller.Create(author1);
                controller.Create(author2);
            });

            UsingSession((session) =>
            {
                WaitForTheLastWrite<Author>(session);

                var author = session.Query<Author>().First(a => a.Name == author2.Name);
                author.Name = author1.Name;

                var controller = new AuthorsController(new Repository(session)); 
                var viewResult = (System.Web.Mvc.ViewResult)(controller.Edit(author));

                Assert.AreEqual("An author with this name already exists", controller.TempData["flashError"]);

                Assert.AreEqual("", viewResult.MasterName);
            });
        }

        [TestMethod]
        public void ShouldListAuthorsWithPagination()
        {
            UsingSession((session) =>
                {
                    var controller = new AuthorsController(new Repository(session));
                    Enumerable.Range(1, 9)
                              .ToList()
                              .ForEach(i => controller.Create(new Author {Name = "Author " + i, Biography = "Biography " + i}));
                });

            UsingSession((session) =>
            {
                WaitForTheLastWrite<Author>(session);
                var controller = new AuthorsController(new Repository(session));
                var viewResult = controller.List(1, 4);
                var authors = (IPagedList<Author>)(viewResult.Model);
                Assert.AreEqual(4, authors.Count);
                Assert.AreEqual(3, authors.PageCount);
            });
        }

        [TestMethod]
        public void DetailsShouldAuthorsBooks()
        {
            var author1 = new Author()
            {
                Name = "Author1",
                Biography = "Biography1",
                PictureUrl = "myPicture1.jpg",
                CreatedAt = DateTime.UtcNow
            };

            var book1 = new Book
            {
                Title = "Oliver Orphan",
                Author = author1.Name,
                AgeRange = "0~2",
                CreatedAt = DateTime.UtcNow
            };
            
            var book2 = new Book
            {
                Title = "Oliver Orphan2",
                Author = author1.Name,
                AgeRange = "0~2",
                CreatedAt = DateTime.UtcNow.AddDays(-1)
            };

            UsingSession((session) =>
            {
                var repository = new Repository(session);
                var controller = new AuthorsController(repository);
                controller.Create(author1);
                
                repository.Create(book1);
                repository.Create(book2);
            });

            using (var session = _documentStore.OpenSession())
            {
                var author = WaitForTheLastWrite<Author>(session);
                var controller = new AuthorsController(new Repository(session));
                var result = (ViewResult) controller.Details(author.Id);
                var authorViewModel = (AuthorViewModel) result.Model;

                AuthorsContollerTestHelper.AssertEqual(authorViewModel.Author, author1);
                var books = authorViewModel.Books;
                Assert.AreEqual(2, books.Count());
                Assert.IsFalse(authorViewModel.HasMoreBooks);

                Assert.AreEqual(book1.Title, books.First().Title);
                Assert.AreEqual(book2.Title, books.Last().Title);
            }
        }

        [TestMethod]
        public void ShouldOnlyListAuthorFirst4BooksInAuthorDetailPage()
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
                var repository = new Repository(session);
                var controller = new AuthorsController(repository);
                controller.Create(author1);
                Enumerable.Range(1, 9)
                          .ToList()
                          .ForEach(i => repository.Create(new Book() { Title = "Book " + i, Author = author1.Name, CreatedAt = DateTime.UtcNow.AddDays(-i)}));
            });

            UsingSession((session) =>
            {
                var author = WaitForTheLastWrite<Author>(session);
                var controller = new AuthorsController(new Repository(session));
                var viewResult = controller.Details(author.Id);
                var authorViewModel = (AuthorViewModel) (viewResult.Model);
                Assert.AreEqual(4, authorViewModel.Books.Count);
                Assert.IsTrue(authorViewModel.HasMoreBooks);
            });
        }

        [TestMethod]
        public void ShouldListAuthorBooksWithPagination()
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
                var repository = new Repository(session);
                var controller = new AuthorsController(repository);
                controller.Create(author1);
                Enumerable.Range(1, 22)
                          .ToList()
                          .ForEach(i => repository.Create(new Book() { Title = "Book " + i, Author = author1.Name, CreatedAt = DateTime.UtcNow.AddDays(-i) }));
            });

            UsingSession((session) =>
            {
                var author = WaitForTheLastWrite<Author>(session);
                var controller = new AuthorsController(new Repository(session));
                var viewResult = controller.Books(author.Id);
                var books = (IPagedList<Book>)(viewResult.Model);
                Assert.AreEqual(9, books.Count);
                Assert.AreEqual(3, books.PageCount);

                viewResult = controller.Books(author.Id, 3, 9);
                books = (IPagedList<Book>)(viewResult.Model);
                Assert.AreEqual(4, books.Count);
                Assert.AreEqual(3, books.PageCount);

                Assert.AreEqual(author.Name, ((Author)(viewResult.ViewBag.Author)).Name);
            });
        }
    }
}