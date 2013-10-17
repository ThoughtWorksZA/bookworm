using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BookWorm.Controllers;
using BookWorm.Models;
using BookWorm.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BookWorm.Tests.Integration.Controllers
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

        [TestMethod]
        public void ShouldDisplayOnlyDraftNewsCreatedByCurrentUserWhenListingDraftNews()
        {
            string userName = "user";

            UsingSession((session) =>
            {
                var repository = new Repository(session);
                var post = new Post { Id = 1, Title = "Post 1", Content = "A Post", UpdatedAt = DateTime.Now.AddMinutes(-1), IsDraft = true, Creator = userName };
                repository.Create(post);
                post = new Post { Id = 2, Title = "Post 2", Content = "A Post", UpdatedAt = DateTime.Now.AddMinutes(-1), IsDraft = true, Creator = userName };
                repository.Create(post);
                post = new Post { Id = 3, Title = "Post 3", Content = "A Post", UpdatedAt = DateTime.Now.AddMinutes(-1), IsDraft = true, Creator = "anotherUser" };
                repository.Create(post);
                var controller = new PostsController(repository);
            });

            UsingSession((session) =>
            {
                WaitForTheLastWrite<Post>(session);

                var httpContextStub = new Mock<HttpContextBase>
                {
                    DefaultValue = DefaultValue.Mock
                };
                var user = Mock.Get(httpContextStub.Object.User);
                user.Setup(u => u.Identity.Name).Returns(userName);

                var controller = new PostsController(new Repository(session));
                controller.ControllerContext = new ControllerContext(httpContextStub.Object, new RouteData(), controller);
                var result = (ViewResult)controller.News(excludeDraft: false);
                var pages = (PagedList.IPagedList<IBasePostInformation>)result.Model;

                Assert.AreEqual(2, pages.Count);
            });
        }
    }
}
