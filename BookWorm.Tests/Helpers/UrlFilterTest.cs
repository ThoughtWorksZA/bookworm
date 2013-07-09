using BookWorm.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace BookWorm.Tests.Helpers
{
    [TestClass]
    public class UrlFilterTest
    {
        [TestMethod]
        public void ShouldFilterInvalidCharactersInPath()
        {
            const string title = @"a<b>c*d%e&f:g\h?i/j";
            Assert.AreEqual("abcdefghij", UrlFilter.FilterInvalidCharacters(title));
        }
    }
}
