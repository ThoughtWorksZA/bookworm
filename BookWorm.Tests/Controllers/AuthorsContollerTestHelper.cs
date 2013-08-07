using BookWorm.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BookWorm.Tests.Controllers
{
    public class AuthorsContollerTestHelper
    {
        public static void AssertEqual(Author expected, Author actual)
        {
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.PictureUrl, actual.PictureUrl);
            Assert.AreEqual(expected.Biography, actual.Biography);
        }
    }
}