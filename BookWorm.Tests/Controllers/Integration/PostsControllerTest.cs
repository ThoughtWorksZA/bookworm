using System;
using System.Web.Mvc;
using BookWorm.Controllers;
using BookWorm.Models;
using BookWorm.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BookWorm.Tests.Controllers.Integration
{
    [TestClass]
    public class PostsControllerTest : ControllerInMemoryRavenTestBase
    {
        [TestMethod]
        public void ShouldExcludeDraftPostsWhenListingAllPosts()
        {
            UsingSession((session) =>
            {
                var repository = new Repository(session);
                var post = new Post { Id = 1, Title = "Post 1", Content = "A Post", UpdatedAt = DateTime.Now.AddMinutes(-1) } ;
                repository.Create(post);
                post = new Post { Id = 2, Title = "Post 2", Content = "A Post", UpdatedAt = DateTime.Now.AddMinutes(-1) } ;
                repository.Create(post);
                post = new Post { Id = 3, Title = "Post 3", Content = "A Post", UpdatedAt = DateTime.Now.AddMinutes(-1), IsDraft = true} ;
                repository.Create(post);
                var controller = new PostsController(repository);
            });

            UsingSession((session) =>
            {
                WaitForTheLastWrite<Post>(session);
                var controller = new PostsController(new Repository(session));
                var result = (ViewResult)controller.News();
                var pages = (PagedList.IPagedList<IBasePostInformation>)result.Model;

                Assert.AreEqual(2, pages.Count);
            });
        }
    }
}
