using System;
using BookWorm.Models;
using BookWorm.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BookWorm.Tests.ViewModels
{
    [TestClass]
    public class BookPostInformationTest
    {
        [TestMethod]
        public void ShouldSummarizeDescription()
        {
            var bookPostInformation =
                new BookPostInformation(1, new BookPost { Content = "__The quick brown fox__ jumps over the lazy dog." });
            Assert.AreEqual("<p><strong>The quick brown fox</strong> jumps</p>\n", bookPostInformation.Summary(24));
        }

        [TestMethod]
        public void ShouldReturnDescriptionIfItSmallerThanTheSummarySize()
        {
            var bookPostInformation =
                new BookPostInformation(1, new BookPost { Content = "The quick brown fox jumps over the lazy dog." });
            Assert.AreEqual("<p>The quick brown fox jumps over the lazy dog.</p>\n", bookPostInformation.Summary(200));
        }

        [TestMethod]
        public void ShouldReturnBookImageFromBook()
        {
            var bookPostInformation =
               new BookPostInformation(1, new BookPost (), new Book {CoverImageUrl = "someurl"});
            Assert.AreEqual("someurl", bookPostInformation.FeaturedImage);
        }
    }
}
