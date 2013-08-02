using BookWorm.Controllers;
using BookWorm.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace BookWorm.Tests.Controllers.Integration
{
    [TestClass]
    public class AuthorControllerTest : ControllerInMemoryRavenTestBase
    {
        [TestMethod]
        public void ShouldCreateNewAuthor()
        {
            var model = new Author()
                {
                    Name = "Author",
                    Biography = "Biography",
                    PictureUrl = "myPicture.jpg"
                };

            UsingSession((session) =>
            {
                var controller = new AuthorController(new Repository(session));

                var actionResult = (System.Web.Mvc.RedirectToRouteResult)(controller.Create(model));
                
                Assert.AreEqual("Author", actionResult.RouteValues["controller"]);
                Assert.AreEqual("List", actionResult.RouteValues["action"]);

            });

            UsingSession((session) =>
                {
                    var author = session.Query<Author>().Customize(a => a.WaitForNonStaleResultsAsOfLastWrite()).First();
                    AssertEqual(model, author);
                });
        }

        private void AssertEqual(Author expected, Author actual)
        {
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.PictureUrl, actual.PictureUrl);
            Assert.AreEqual(expected.Biography, actual.Biography);
        }
    }
}
