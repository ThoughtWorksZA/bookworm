using BookWorm.Controllers;
using BookWorm.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BookWorm.Tests.Controllers
{
    [TestClass]
    public class BooksControllerTest
    {
        [TestMethod]
        public void ShouldReturnCreatePageOnGetCreate()
        {
            var booksController = new BooksController();

            var createABookPage = booksController.New();
            var model = createABookPage.Model;

            Assert.AreEqual("Add a Book", booksController.ViewBag.Title);
            Assert.IsInstanceOfType(model, typeof(Book));
        }

        [TestMethod]
        public void ShouldReturnPageWhenBookIsCreated()
        {
            var book = new Book { Title = "The Book" };

            var mockedRepo = new Mock<Repository>();
            mockedRepo.Setup(repo => repo.Create(book));
            var booksController = new BooksController(mockedRepo.Object);

            var viewResult = booksController.New(book);

            Assert.IsNotNull(viewResult);
            Assert.AreEqual("Created book 'The Book'", booksController.ViewBag.SuccessMessage);
            Assert.AreEqual("Success", booksController.ViewBag.Title);
        }

        [TestMethod]
        public void ShouldUseRepositoryWhenCreatingABook()
        {
            var book = new Book();
            var mockedRepo = new Mock<Repository>();
            mockedRepo.Setup(repo => repo.Create(book));

            var booksController = new BooksController(mockedRepo.Object);

            booksController.New(book);
            mockedRepo.Verify(repo => repo.Create(book));
        }
    }
}
