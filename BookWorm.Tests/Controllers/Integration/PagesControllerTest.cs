using System;
using BookWorm.Controllers;
using BookWorm.Models;
using BookWorm.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BookWorm.Tests.Controllers.Integration
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
    }
}
