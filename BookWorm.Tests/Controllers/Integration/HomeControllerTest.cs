using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookWorm.Controllers;
using BookWorm.Models;
using BookWorm.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Raven.Client;
using Raven.Client.Embedded;

namespace BookWorm.Tests.Controllers.Integration
{
    [TestClass]
    public class HomeControllerTest
    {
        private IDocumentStore _documentStore;

        [TestInitialize]
        public void Setup()
        {
            _documentStore = new EmbeddableDocumentStore
            {
                RunInMemory = true
            };
            _documentStore.Initialize();
        }

        [TestMethod]
        public void IndexShouldHave5LatestPostsOrBookPosts()
        {
            using (var session = _documentStore.OpenSession())
            {
                var repository = new Repository(session);
                repository.Create(new Post { Id = 1, Title = "Post", Content = "Derp", CreatedAt = DateTime.Now.AddMinutes(1) });
                repository.Create(new Book { Posts = new List<BookPost> { new BookPost { Id = 2, Title = "BookPost", Content = "Derp", Type = BookPost.BookPostType.News, UpdatedAt = DateTime.Now.AddMinutes(1) } } });
                repository.Create(new Post { Id = 3, Title = "Post", Content = "Derp", CreatedAt = DateTime.Now.AddMinutes(2) });
                repository.Create(new Book { Posts = new List<BookPost> { new BookPost { Id = 4, Title = "BookPost", Content = "Derp", Type = BookPost.BookPostType.Events, UpdatedAt = DateTime.Now.AddMinutes(2) } } });
                repository.Create(new Post { Id = 5, Title = "Post", Content = "Derp", CreatedAt = DateTime.Now.AddMinutes(3) });
                repository.Create(new Book { Posts = new List<BookPost> { new BookPost { Id = 6, Title = "BookPost", Content = "Derp", Type = BookPost.BookPostType.News, UpdatedAt = DateTime.Now.AddMinutes(3) } } });
                var post = new Post { Id = 101, Title = "Latest Post", Content = "Derp", CreatedAt = DateTime.Now.AddMinutes(10) };
                repository.Create(post);
                var bookPost = new Book { Posts = new List<BookPost> { new BookPost { Id = 102, Title = "Latest BookPost", Content = "Derp", Type = BookPost.BookPostType.Events, UpdatedAt = DateTime.Now.AddMinutes(4) } } };
                repository.Create(bookPost);
                repository.Create(new Post { Id = 7, Title = "Post", Content = "Derp", CreatedAt = DateTime.Now.AddMinutes(4) });
                repository.Create(new Book { Posts = new List<BookPost> { new BookPost { Id = 8, Title = "BookPost", Content = "Derp", Type = BookPost.BookPostType.TeachingGuides, UpdatedAt = DateTime.Now.AddMinutes(4) } } });
                repository.Create(new Post { Id = 9, Title = "Post", Content = "Derp", CreatedAt = DateTime.Now.AddMinutes(5) });
                repository.Create(new Book { Posts = new List<BookPost> { new BookPost { Id = 10, Title = "BookPost", Content = "Derp", Type = BookPost.BookPostType.Reviews, UpdatedAt = DateTime.Now.AddMinutes(5) } } });
                session.SaveChanges();
                var controller = new HomeController(repository);
                controller.Index();
                Assert.AreEqual(5, ((List<PostInformation>)controller.ViewBag.PostInformations).Count() + ((List<BookPostInformation>)controller.ViewBag.BookPostInformations).Count());
                Assert.AreEqual(101, ((List<PostInformation>) controller.ViewBag.PostInformations).First().Model.Id);
                Assert.AreEqual(102, ((List<BookPostInformation>) controller.ViewBag.BookPostInformations).First().Model.Id);
            }
        }
    }
}
