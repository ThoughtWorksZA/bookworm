using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BookWorm.Controllers;
using BookWorm.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BookWorm.Tests.Controllers
{
    [TestClass]
    public class BookPostsControllerTest : BaseControllerTest
    {
        [TestMethod]
        public void ShouldListBookPosts()
        {
            var repository = new Mock<Repository>();
            var savedPages = new List<BookPost> 
                {
                    new BookPost { Id = 1, Title = "test title", Content = "Hello\n=====\nWorld" }, 
                    new BookPost { Id = 2, Title = "test title2", Content = "Hello\n=====\nAnother World" }
                };
            repository.Setup(repo => repo.List<BookPost>()).Returns(savedPages);
            var controller = new BookPostsController(repository.Object);
            var result = controller.List();
            repository.Verify(it => it.List<BookPost>(), Times.Once());
            Assert.AreEqual(savedPages, result.Model);
        }

        [TestMethod]
        public void ShouldReturnCreatePageOnGetCreate()
        {

            var controller = new BookPostsController();
            var createABookPostView = (ViewResult) controller.Create();
            var model = createABookPostView.Model;

            Assert.AreEqual("Add a Book Post", controller.ViewBag.Title);
            Assert.IsInstanceOfType(model, typeof(BookPost));
        }

        [TestMethod]
        public void ShouldStoreBookPostWhenCreated()
        {
            var repository = new Mock<Repository>();
            var submittedBookPost = new BookPost { Title = "test title", Content = "some content" };
            repository.Setup(repo => repo.Create(submittedBookPost)).Returns(new BookPost { Id = 1, Title = submittedBookPost.Title, Content = submittedBookPost.Content });
            var controller = new BookPostsController(repository.Object);
            var result = (RedirectToRouteResult)controller.Create(submittedBookPost);
            repository.Verify(it => it.Create(submittedBookPost), Times.Once());
            Assert.AreEqual(string.Format("Added {0} successfully", submittedBookPost.Title), controller.TempData["flashSuccess"]);
            Assert.AreEqual(1, result.RouteValues["id"]);
        }

        [TestMethod]
        public void ShouldNotStoreBookPostWhenTitleIsInvalid()
        {
            var repository = new Mock<Repository>();
            var submittedBookPost = new BookPost { Title = "", Content = "some content", Type = BookPost.BookPostType.BookReview };
            repository.Setup(repo => repo.Create(submittedBookPost)).Returns(submittedBookPost);

            var controller = new BookPostsController(repository.Object);
            ValidateViewModel(submittedBookPost, controller);
            Assert.IsFalse(controller.ModelState.IsValid);
        }

        [TestMethod]
        public void ShouldNotStoreBookPostWhenContentIsInvalid()
        {
            var repository = new Mock<Repository>();
            var submittedBookPost = new BookPost { Title = "test title", Content = "", Type = BookPost.BookPostType.BookReview};
            repository.Setup(repo => repo.Create(submittedBookPost)).Returns(submittedBookPost);

            var controller = new BookPostsController(repository.Object);
            ValidateViewModel(submittedBookPost, controller);
            Assert.IsFalse(controller.ModelState.IsValid);
        }

        [TestMethod]
        public void BookPostsControllerCreateShouldOnlyAllowHttpPost()
        {
            var bookPostsControllerClass = typeof(BookPostsController);
            Assert.AreEqual(1, bookPostsControllerClass.GetMethods()
                                                   .First(method => method.Name == "Create" && method.GetParameters().Any())
                                                   .GetCustomAttributes(typeof(HttpPostAttribute), false)
                                                   .Count());
        }

        [TestMethod]
        public void ShouldNotStoreBookPostWhenTypeIsInvalid()
        {
            var repository = new Mock<Repository>();
            var submittedBookPost = new BookPost { Title = "test title", Content = "derp" };
            repository.Setup(repo => repo.Create(submittedBookPost)).Returns(submittedBookPost);

            var controller = new BookPostsController(repository.Object);
            ValidateViewModel(submittedBookPost, controller);
            Assert.IsFalse(controller.ModelState.IsValid);
        }

        [TestMethod]
        public void ShouldRedirectToDetailsPageWhenBookPostIsCreated()
        {
            var bookPost = new BookPost { Title = "The Book Post", Content = "some content", Type = BookPost.BookPostType.BookReview};

            var mockedRepo = new Mock<Repository>();
            mockedRepo.Setup(repo => repo.Create(bookPost)).Returns(new BookPost { Id = 1, Title = "The Book Post", Content = "some content", Type = BookPost.BookPostType.BookReview });
            var bookPostsController = new BookPostsController(mockedRepo.Object);

            var viewResult = (RedirectToRouteResult)bookPostsController.Create(bookPost);

            Assert.IsNotNull(viewResult);
            Assert.AreEqual("Added The Book Post successfully", bookPostsController.TempData["flashSuccess"]);
            Assert.AreEqual(1, viewResult.RouteValues["id"]);
        }

        [TestMethod]
        public void CreateBookPostShouldNotSaveWhenBookPostIsInvalid()
        {
            var bookPost = new BookPost();
            var mockedRepo = new Mock<Repository>();
            var bookPostsController = new BookPostsController(mockedRepo.Object);
            mockedRepo.Setup(repo => repo.Create(bookPost)).Returns(bookPost);
            bookPostsController.ModelState.AddModelError("test error", "test exception");

            var result = (ViewResult)bookPostsController.Create(bookPost);

            mockedRepo.Verify(repo => repo.Create(bookPost), Times.Never(), "failing model validation should prevent creating book post");
            Assert.AreEqual("There were problems saving this book post", bookPostsController.TempData["flashError"]);
        }


    }
}
