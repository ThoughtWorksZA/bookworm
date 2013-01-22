using System;
using System.Collections.Generic;
using System.Web.Mvc;
using BookWorm.Controllers;
using BookWorm.Models;
using BookWorm.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BookWorm.Tests.Controllers
{
    [TestClass]
    public class PostsControllerTest
    {
        [TestMethod]
        public void ShouldHavePostsAndBookPosts()
        {
            var posts = new List<Post> {new Post {Id = 1, Title = "Post", Content = "A Post", UpdatedAt = DateTime.Now.AddMinutes(-1)}};
            var books = new List<Book>
                {
                    new Book
                        {
                            Posts = new List<BookPost> {new BookPost {Id = 2, Title = "BookPost", Content = "A BookPost", Type = BookPost.BookPostType.News, UpdatedAt = DateTime.Now}}
                        }
                };
            var repo = new Mock<Repository>();
            repo.Setup(r => r.List<Post>()).Returns(posts);
            repo.Setup(r => r.List<Book>()).Returns(books);
            var controller = new PostsController(repo.Object);
            var result = (ViewResult)controller.News();
            var model = (PagedList.IPagedList<IBasePostInformation>) result.Model;
            Assert.AreEqual(2, model.Count);
            Assert.AreEqual(2, model[0].Model.Id);
            Assert.AreEqual(1, model[1].Model.Id);
        }

        [TestMethod]
        public void ShouldOnlyHaveNewsAndEventsBookPosts()
        {
            var books = new List<Book>
                {
                    new Book
                        {
                            Posts = new List<BookPost>
                                {
                                    new BookPost {Id = 1, Title = "BookPost", Content = "A BookPost", Type = BookPost.BookPostType.News, UpdatedAt = DateTime.Now},
                                    new BookPost {Id = 2, Title = "BookPost", Content = "A BookPost", Type = BookPost.BookPostType.Events, UpdatedAt = DateTime.Now.AddMinutes(-1)},
                                    new BookPost {Id = 3, Title = "BookPost", Content = "A BookPost", Type = BookPost.BookPostType.TeachingGuides}
                                }
                        }
                };
            var repo = new Mock<Repository>();
            repo.Setup(r => r.List<Post>()).Returns(new List<Post>());
            repo.Setup(r => r.List<Book>()).Returns(books);
            var controller = new PostsController(repo.Object);
            var result = (ViewResult)controller.News();
            var model = (PagedList.IPagedList<IBasePostInformation>) result.Model;
            Assert.AreEqual(2, model.Count);
            Assert.AreEqual(1, model[0].Model.Id);
            Assert.AreEqual(2, model[1].Model.Id);
        }
    }
}
