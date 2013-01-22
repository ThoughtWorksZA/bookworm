using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BookWorm.Controllers;
using BookWorm.Models;
using BookWorm.ViewModels;
using MarkdownSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Raven.Client;

namespace BookWorm.Tests.Controllers
{
    [TestClass]
    public class BookPostsControllerTest : BaseControllerTest
    {
        [TestMethod]
        public void ShouldReturnCreatePageOnGetCreate()
        {
            var book = new Book { Id = 1, Title = "A book" };
            var mockedRepo = new Mock<Repository>();
            mockedRepo.Setup(repo => repo.Get<Book>(book.Id)).Returns(book);
            var controller = new BookPostsController(mockedRepo.Object);
            var createABookPostView = (ViewResult) controller.Create(1);
            var model = (BookPostInformation)createABookPostView.Model;

            Assert.AreEqual("Add a Book Post", controller.ViewBag.Title);
            Assert.IsInstanceOfType(model, typeof(BookPostInformation));
            Assert.AreEqual(book.Id, model.BookId);
        }

        [TestMethod]
        public void ShouldStoreBookPostWhenCreated()
        {
            var repository = new Mock<Repository>();
            var book = new Book { Id = 1 };
            var bookPost = new BookPost {Title = "test title", Content = "some content"};
            var submittedBookPostInformation = new BookPostInformation(book.Id, bookPost);
            repository.Setup(repo => repo.Get<Book>(book.Id)).Returns(book);
            var controller = new BookPostsController(repository.Object);
            var result = (RedirectToRouteResult)controller.Create(submittedBookPostInformation);
            repository.Verify(it => it.Edit(book), Times.Once());
            Assert.AreEqual(string.Format("Added {0} successfully", bookPost.Title), controller.TempData["flashSuccess"]);
            Assert.AreEqual(book.Id, result.RouteValues["id"]);
        }

        [TestMethod]
        public void ShouldNotStoreBookPostWhenTitleIsInvalid()
        {
            var repository = new Mock<Repository>();
            var submittedBookPost = new BookPost { Title = "", Content = "some content", Type = BookPost.BookPostType.Reviews };
            repository.Setup(repo => repo.Create(submittedBookPost)).Returns(submittedBookPost);

            var controller = new BookPostsController(repository.Object);
            ValidateViewModel(submittedBookPost, controller);
            Assert.IsFalse(controller.ModelState.IsValid);
        }

        [TestMethod]
        public void ShouldNotStoreBookPostWhenContentIsInvalid()
        {
            var repository = new Mock<Repository>();
            var submittedBookPost = new BookPost { Title = "test title", Content = "", Type = BookPost.BookPostType.Reviews};
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
                                                   .First(method => method.Name == "Create" && method.GetParameters().First().ParameterType == typeof(BookPostInformation))
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
            var repository = new Mock<Repository>();
            var book = new Book { Id = 1 };
            var bookPost = new BookPost { Title = "The Book Post", Content = "some content", Type = BookPost.BookPostType.Reviews };
            var submittedBookPostInformation = new BookPostInformation(book.Id, bookPost);
            repository.Setup(repo => repo.Get<Book>(book.Id)).Returns(book);
            var bookPostsController = new BookPostsController(repository.Object);
            var viewResult = (RedirectToRouteResult)bookPostsController.Create(submittedBookPostInformation);

            repository.Verify(repo => repo.Edit(book));

            Assert.IsNotNull(viewResult);
            Assert.AreEqual("Added The Book Post successfully", bookPostsController.TempData["flashSuccess"]);
            Assert.AreEqual(1, viewResult.RouteValues["id"]);
            Assert.IsTrue(book.Posts.Contains(bookPost));
        }

        [TestMethod]
        public void CreateBookPostShouldNotSaveWhenBookPostIsInvalid()
        {
            var repository = new Mock<Repository>();
            var book = new Book { Id = 1 };
            var bookPost = new BookPost { Title = "The Book Post", Content = "some content", Type = BookPost.BookPostType.Reviews };
            var submittedBookPostInformation = new BookPostInformation(book.Id, bookPost);
            repository.Setup(repo => repo.Get<Book>(book.Id)).Returns(book);
            var bookPostsController = new BookPostsController(repository.Object);
            bookPostsController.ModelState.AddModelError("test error", "test exception");

            var result = (ViewResult)bookPostsController.Create(submittedBookPostInformation);

            repository.Verify(repo => repo.Edit(book), Times.Never(), "failing model validation should prevent creating book post");
            Assert.AreEqual("There were problems saving this book post", bookPostsController.TempData["flashError"]);
        }

        [TestMethod]
        public void ShouldKnowBookPostsControllerRequiresAuthorization()
        {
            var pagesControllerClass = typeof(BookPostsController);
            Assert.AreEqual(1, pagesControllerClass.GetCustomAttributes(typeof(AuthorizeAttribute), false).Count());
        }

        [TestMethod]
        public void ShouldKnowBookPostsControllerDetailsAllowsAnonymous()
        {
            var pagesControllerClass = typeof(BookPostsController);
            Assert.AreEqual(1, pagesControllerClass.GetMethods()
                                                   .First(method => method.Name == "Details")
                                                   .GetCustomAttributes(typeof(AllowAnonymousAttribute), false)
                                                   .Count());
        }

        [TestMethod]
        public void ShouldKnowHowToDisplayABookPost()
        {
            var id = 12;
            var repository = new Mock<Repository>();
            var book = new Book { Id = 1 };
            var savedBookPost = new BookPost { Id = id, Title = "test title", Content = "some content", Type = BookPost.BookPostType.Reviews};
            book.Posts.Add(savedBookPost);
            repository.Setup(repo => repo.Get<Book>(book.Id)).Returns(book);
            var controller = new BookPostsController(repository.Object);
            var result = controller.Details(id, book.Id);
            repository.Verify(it => it.Get<Book>(book.Id), Times.Once());
            Assert.AreEqual(book.Id, ((BookPostInformation)result.Model).BookId);
            Assert.AreEqual(id, ((BookPostInformation)result.Model).Model.Id);
            Assert.AreEqual(book, ((BookPostInformation)result.Model).Book);
        }

        [TestMethod]
        public void ShouldKnowHowToRenderAnEditPage()
        {
            var repositoryMock = new Mock<Repository>();
            var bookPost = new BookPost { Id = 1, Title = "The Page", Type = BookPost.BookPostType.Reviews };
            var book = new Book {Id = 1};
            book.Posts.Add(bookPost);
            repositoryMock.Setup(repo => repo.Get<Book>(book.Id)).Returns(book);
            var bookPostsController = new BookPostsController(repositoryMock.Object);

            var result = bookPostsController.Edit(bookPost.Id, book.Id);
            var actualModel = (BookPostInformation)result.Model;

            Assert.AreEqual(bookPost.Title, actualModel.Model.Title);
            Assert.AreEqual("PUT", bookPostsController.ViewBag.Method);
            repositoryMock.Verify(repo => repo.Get<Book>(book.Id), Times.Once());
        }

        [TestMethod]
        public void ShouldKnowHowToUpdateABookPost()
        {
            var repository = new Mock<Repository>();
            var book = new Book {Id = 1};
            var existingBookPost = new BookPost { Id = 1, Title = "Derping for dummies", Type = BookPost.BookPostType.Reviews };
            book.Posts.Add(existingBookPost);
            repository.Setup(repo => repo.Get<Book>(book.Id)).Returns(book);
            var bookPostsController = new BookPostsController(repository.Object);
            var result = (RedirectToRouteResult)bookPostsController.Edit(new BookPostInformation(book.Id, existingBookPost));
            Assert.AreEqual(existingBookPost.Id, result.RouteValues["id"]);
            Assert.AreEqual("Updated Derping for dummies successfully", bookPostsController.TempData["flashSuccess"]);
            repository.Verify(repo => repo.Edit(book), Times.Once());
        }

        [TestMethod]
        public void ShouldSetCreatedAtFromExisitingBookPost()
        {
            var repository = new Mock<Repository>();
            var book = new Book { Id = 1 };
            var existingBookPost = new BookPost { Id = 1, Title = "Derping for dummies", Type = BookPost.BookPostType.Reviews, CreatedAt = DateTime.Now.AddMinutes(-1)};
            book.Posts.Add(existingBookPost);
            repository.Setup(repo => repo.Get<Book>(book.Id)).Returns(book);
            var bookPostsController = new BookPostsController(repository.Object);
            var editedBookPost = new BookPost { Id = 1, Title = "Derping for dummies II", Type = BookPost.BookPostType.Reviews, CreatedAt = DateTime.Now };
            bookPostsController.Edit(new BookPostInformation(book.Id, editedBookPost));
            Assert.AreEqual(existingBookPost.CreatedAt, editedBookPost.CreatedAt);
        }

        [TestMethod]
        public void EditBookShouldNotSaveWhenBookIsInvalid()
        {
            var bookPost = new BookPost();
            var book = new Book { Id = 1 };
            book.Posts.Add(bookPost);
            var mockedRepo = new Mock<Repository>();
            var bookPostsController = new BookPostsController(mockedRepo.Object);
            mockedRepo.Setup(repo => repo.Get<Book>(book.Id)).Returns(book);
            bookPostsController.ModelState.AddModelError("test error", "test exception");

            var result = (ViewResult)bookPostsController.Edit(new BookPostInformation(book.Id, bookPost));

            mockedRepo.Verify(repo => repo.Edit(book), Times.Never(), "failing model validation should prevent updating book post");
            Assert.AreEqual("There were problems saving this book post", bookPostsController.TempData["flashError"]);
        }

        [TestMethod]
        public void BookPostsControllerEditShouldOnlyAllowHttpPut()
        {
            var bookPostsControllerClass = typeof(BookPostsController);
            Assert.AreEqual(1, bookPostsControllerClass.GetMethods()
                                                   .First(method => method.Name == "Edit" && method.GetParameters().First().ParameterType == typeof(BookPostInformation))
                                                   .GetCustomAttributes(typeof(HttpPutAttribute), false)
                                                   .Count());
        }

        [TestMethod]
        public void ShouldDeleteBookPostAndShowListOfBooksPosts()
        {
            var mockedRepo = new Mock<Repository>();
            var book = new Book {Id = 1};
            var bookPost = new BookPost {Id = 2, Title = "something"};
            book.Posts.Add(bookPost);
            mockedRepo.Setup(repo => repo.Get<Book>(book.Id)).Returns(book);
            var bookPostsController = new BookPostsController(mockedRepo.Object);

            var viewResult = bookPostsController.Delete(bookPost.Id, book.Id);
            mockedRepo.Verify(repo => repo.Edit(book));
            Assert.AreEqual("Book Post successfully deleted", bookPostsController.TempData["flashSuccess"]);
            Assert.AreEqual("Details", viewResult.RouteValues["action"]);
            Assert.AreEqual(book.Id, viewResult.RouteValues["id"]);
        }

        [TestMethod]
        public void BookPostsControllerDeleteShouldOnlyAllowHttpDelete()
        {
            var bookPostsControllerClass = typeof(BookPostsController);
            Assert.AreEqual(1, bookPostsControllerClass.GetMethods()
                                                   .First(method => method.Name == "Delete")
                                                   .GetCustomAttributes(typeof(HttpDeleteAttribute), false)
                                                   .Count());
        }

        [TestMethod]
        public void ShouldKnowToRenderTheBookPostContentAsMarkdown()
        {
            var id = 12;
            var repository = new Mock<Repository>();
            var book = new Book { Id = 1 };
            var markdown = new Markdown();
            var savedBookPost = new BookPost { Id = id, Title = "test title", Content = "Hello\n=====\nWorld", Type = BookPost.BookPostType.Reviews };
            book.Posts.Add(savedBookPost);
            repository.Setup(repo => repo.Get<Book>(book.Id)).Returns(book);
            var controller = new BookPostsController(repository.Object);
            var result = controller.Details(id, book.Id);
            repository.Verify(it => it.Get<Book>(book.Id), Times.Once());
            var transformedContent = markdown.Transform(savedBookPost.Content);
            Assert.AreEqual(transformedContent, result.ViewBag.transformedContent);
            Assert.AreEqual(id, ((BookPostInformation)result.Model).Model.Id);
        }
    }
}
