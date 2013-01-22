using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using BookWorm.Models;
using BookWorm.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BookWorm;
using BookWorm.Controllers;
using Moq;

namespace BookWorm.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void About()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.About() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Contact()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Contact() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void IndexShouldHave4LatestUpdatedBooks()
        {
            var books = new List<Book> {new Book(), new Book(), new Book(), new Book()};
            var mockedRepo = new Mock<Repository>();
            mockedRepo.Setup(repo => repo.List<Book>(4)).Returns(books);
            mockedRepo.Setup(repo => repo.List<Post>(5)).Returns(new List<Post>());
            mockedRepo.Setup(repo => repo.List<Book>()).Returns(new List<Book>());
            var controller = new HomeController(mockedRepo.Object);
            var result = (ViewResult)controller.Index();
            var model = (HomeViewModel)result.Model;
            Assert.AreEqual(4, model.Books.Count());
        }
    }
}
