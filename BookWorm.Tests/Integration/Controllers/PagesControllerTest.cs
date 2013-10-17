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
    public class PagesControllerTest : ControllerInMemoryRavenTestBase
    {
        [TestMethod]
        public void ShouldExcludeDraftPagesWhenListingAllPages()
        {
            UsingSession((session) =>
            {
                var repository = new Repository(session);
                var controller = new PagesController(repository);
                controller.Create(new StaticPageInformation()
                    {
                        Model = new StaticPage { Id = 1, Title = "test title", Content = "Hello\n=====\nWorld" }
                    });
                controller.Create(new StaticPageInformation()
                    {
                        Model = new StaticPage { Id = 2, Title = "test title2", Content = "Hello\n=====\nAnother World" }
                    });
                controller.Create(new StaticPageInformation()
                    {
                        Model = new StaticPage { Id = 3, Title = "Draft Page", Content = "Hello\n=====\nI'm a draft", IsDraft = true }
                    });
            });

            UsingSession((session) =>
            {
                WaitForTheLastWrite<StaticPage>(session);
                var controller = new PagesController(new Repository(session));
                var result = controller.List();
                var pages = (PagedList.IPagedList<StaticPageInformation>)result.Model;

                Assert.AreEqual(2, pages.Count);
            });
        }

        [TestMethod]
        public void ShouldDisplayOnlyDraftPagesCreatedByCurrentUserWhenListingDraftPages()
        {
            string userName = "user";

            UsingSession((session) =>
            {
                var repository = new Repository(session);
                var controller = new PagesController(repository);
                controller.Create(new StaticPageInformation()
                {
                    Model = new StaticPage { Id = 1, Title = "Draft Page", Content = "Hello\n=====\nWorld", IsDraft = true, Creator = userName}
                });
                controller.Create(new StaticPageInformation()
                {
                    Model = new StaticPage { Id = 2, Title = "Draft Page 2", Content = "Hello\n=====\nAnother World", IsDraft = true, Creator = userName}
                });
                controller.Create(new StaticPageInformation()
                {
                    Model = new StaticPage { Id = 3, Title = "Draft Page 3", Content = "Hello\n=====\nI'm a draft", IsDraft = true, Creator = "anotherUser" }
                });
            });

            UsingSession((session) =>
            {
                WaitForTheLastWrite<StaticPage>(session);

                var httpContextStub = new Mock<HttpContextBase>
                {
                    DefaultValue = DefaultValue.Mock
                };
                var user = Mock.Get(httpContextStub.Object.User);
                user.Setup(u => u.Identity.Name).Returns(userName);

                var controller = new PagesController(new Repository(session));
                controller.ControllerContext = new ControllerContext(httpContextStub.Object, new RouteData(), controller);
                var result = controller.List(excludeDraft:false);
                var pages = (PagedList.IPagedList<StaticPageInformation>)result.Model;

                Assert.AreEqual(2, pages.Count);
            });
        }
    }
}
