using BookWorm.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace BookWorm.Tests.Helpers
{
    [TestClass]
    public class UrlUtilsTest
    {
        [TestMethod]
        public void ShouldFilterInvalidCharactersInPath()
        {
            const string title = @"a<b>c*d%e&f:g\h?i/j";
            Assert.AreEqual("abcdefghij", UrlUtils.ConvertTitleForUrl(title));
        }

        [TestMethod]
        public void ShouldJoinTitleWordsWithHyphen()
        {
            const string title = @"Oliver Orphan";
            Assert.AreEqual("Oliver-Orphan", UrlUtils.ConvertTitleForUrl(title));
        }

        [TestMethod]
        public void ShouldJoinTitleWordsWith2NeighborSpacesWith1Hyphen()
        {
            const string title = @"Oliver< Orphan";
            Assert.AreEqual("Oliver-Orphan", UrlUtils.ConvertTitleForUrl(title));
        }

        [TestMethod]
        public void ShouldReturnUrlFromHttpBookContact()
        {
            const string httpBookContact = "http://youshould.buy.this.book.com";
            Assert.AreEqual(@"<a href=""http://youshould.buy.this.book.com"">http://youshould.buy.this.book.com</a>", UrlUtils.BuyBookContactHtml(httpBookContact));
        }

        [TestMethod]
        public void ShouldReturnUrlFromWwwBookContact()
        {
            const string httpBookContact = "www.youshould.buy.this.book.com";
            Assert.AreEqual(@"<a href=""http://www.youshould.buy.this.book.com"">http://www.youshould.buy.this.book.com</a>", UrlUtils.BuyBookContactHtml(httpBookContact));
        }

        [TestMethod]
        public void ShouldReturnPlainTextFromCellNumberBookContact()
        {
            const string httpBookContact = "0823324565";
            Assert.AreEqual("<em>0823324565</em>", UrlUtils.BuyBookContactHtml(httpBookContact));
        }
    }
}
