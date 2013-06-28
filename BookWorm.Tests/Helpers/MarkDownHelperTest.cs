using BookWorm.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace BookWorm.Tests.Helpers
{
    [TestClass]
    public class MarkDownHelperTest
    {
        [TestMethod]
        public void ShouldSummarizeDescription()
        {
            const string content = "__The quick brown fox__ jumps over the lazy dog.";
            Assert.AreEqual("<p><strong>The quick brown fox</strong> jumps</p>\n", MarkDownHelper.Summary(content, 24));
        }

        [TestMethod]
        public void ShouldReturnDescriptionIfItSmallerThanTheSummarySize()
        {
            const string content = "The quick brown fox jumps over the lazy dog.";
            Assert.AreEqual("<p>The quick brown fox jumps over the lazy dog.</p>\n", MarkDownHelper.Summary(content, 200));
        }

        [TestMethod]
        public void ShouldIgnoreImagesInTheDescriptionWhileSummarize()
        {
            const string content = "__The ![enter image description here][2]quick brown fox__ jumps over the lazy dog.";
            Assert.AreEqual("<p><strong>The quick brown fox</strong> jumps</p>\n", MarkDownHelper.Summary(content, 24));
        }

        [TestMethod]
        public void ShouldIgnoreImagesWithTitleInTheDescriptionWhileSummarize()
        {
            const string content = "__The ![image title][1]quick brown fox__ jumps over the lazy dog.";
            Assert.AreEqual("<p><strong>The quick brown fox</strong> jumps</p>\n", MarkDownHelper.Summary(content, 24));
        }

        [TestMethod]
        public void ShouldItalicIfTheClosingAsteriskIsTruncated()
        {
            const string content = "*The quick brown fox jumps over the lazy dog.*";
            Assert.AreEqual("<p><em>The quick brown fox</em></p>\n", MarkDownHelper.Summary(content, 20));
        }

        [TestMethod]
        public void ShouldRemoveTheEndingReturnCharAndBracket()
        {
            const string content = "The quick brown fox jumps over\n> the lazy dog.";
            Assert.AreEqual("<p>The quick brown fox jumps over</p>\n", MarkDownHelper.Summary(content, 30));
        }

        [TestMethod]
        public void ShouldNotRemoveTheEndingHtmlTag()
        {
            const string content = "The quick brown fox jumps over<br/> the lazy dog.";
            Assert.AreEqual("<p>The quick brown fox jumps over<br/></p>\n", MarkDownHelper.Summary(content, 30));
        }
    }
}
