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

        [TestMethod]
        public void ShouldRemoveAsterisksInMetaDescriptionSummary()
        {
            const string content = "*Michael Knight's car is amazing.*";
            Assert.AreEqual("Michael Knight's car is amazing.", MarkDownHelper.SummaryForMetaDescription(content, 32));
        }

        [TestMethod]
        public void ShouldRemoveUnderscoresInMetaDescriptionSummary()
        {
            const string content = "__Michael Knight's__ car is amazing.";
            Assert.AreEqual("Michael Knight's car is amazing.", MarkDownHelper.SummaryForMetaDescription(content, 32));
        }

        [TestMethod]
        public void ShouldRemoveEndingCharReturnValueInMetaDescriptionSummary()
        {
            const string content = "Michael Knight's\n> car is amazing.";
            Assert.AreEqual("Michael Knight's car is amazing.", MarkDownHelper.SummaryForMetaDescription(content, 32));
        }

        [TestMethod]
        public void ShouldTruncateTheExtraWordsInMetaDescriptionSummary()
        {
            const string content = "Michael Knight's\n> car is amazing.";
            Assert.AreEqual("Michael Knight's", MarkDownHelper.SummaryForMetaDescription(content, 10));
        }

        [TestMethod]
        public void ShouldIgnoreImagesInMetaDescriptionSummary()
        {
            const string content = "![enter image description here][2]Michael Knight's\n> car is amazing.";
            Assert.AreEqual("Michael Knight's", MarkDownHelper.SummaryForMetaDescription(content, 10));
        }

        [TestMethod]
        public void ShouldTransformMarkdownString()
        {
            const string content = "__The quick brown fox__ jumps over the lazy dog.";
            Assert.AreEqual("<p><strong>The quick brown fox</strong> jumps over the lazy dog.</p>\n", MarkDownHelper.Transform(content));
        }
    }
}
