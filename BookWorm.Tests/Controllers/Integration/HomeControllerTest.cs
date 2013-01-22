using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
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
                var book = new Book { Posts = new List<BookPost> { new BookPost { Id = 102, Title = "Latest BookPost", Content = "Derp", Type = BookPost.BookPostType.Events, UpdatedAt = DateTime.Now.AddMinutes(5) } } };
                repository.Create(book);
                repository.Create(new Post { Id = 7, Title = "Post", Content = "Derp", CreatedAt = DateTime.Now.AddMinutes(4) });
                repository.Create(new Book { Posts = new List<BookPost> { new BookPost { Id = 8, Title = "BookPost", Content = "Derp", Type = BookPost.BookPostType.TeachingGuides, UpdatedAt = DateTime.Now.AddMinutes(5) } } });
                repository.Create(new Post { Id = 9, Title = "Post", Content = "Derp", CreatedAt = DateTime.Now.AddMinutes(6) });
                repository.Create(new Book { Posts = new List<BookPost> { new BookPost { Id = 10, Title = "BookPost", Content = "Derp", Type = BookPost.BookPostType.Reviews, UpdatedAt = DateTime.Now.AddMinutes(6) } } });
                session.SaveChanges();
                var controller = new HomeController(repository);
                var result = (ViewResult)controller.Index();
                var homeViewModel = (HomeViewModel)result.Model;
                Assert.AreEqual(5, homeViewModel.News.Count());
                var ids = homeViewModel.News.Select(x => x.Model.Id).ToList();
                Assert.IsTrue(ids.Contains(post.Id));
                Assert.IsTrue(ids.Contains(book.Posts.First().Id));
            }
        }
    }
}
