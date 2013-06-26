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
            Assert.AreEqual("<p><strong>The quick brown fox</strong> jumps</p>\n", MarkDownHelper.MarkDownSummary(content, 24));
        }

        [TestMethod]
        public void ShouldReturnDescriptionIfItSmallerThanTheSummarySize()
        {
            const string content = "The quick brown fox jumps over the lazy dog.";
            Assert.AreEqual("<p>The quick brown fox jumps over the lazy dog.</p>\n", MarkDownHelper.MarkDownSummary(content, 200));
        }
    }
}
