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
                new BookPostInformation(1, new BookPost { Content = "The quick brown fox jumps over the lazy dog." });
            Assert.AreEqual("The quick brown fox jumps", bookPostInformation.Summary(20));
        }

        [TestMethod]
        public void ShouldReturnDescriptionIfItSmallerThanTheSummarySize()
        {
            var bookPostInformation =
                new BookPostInformation(1, new BookPost { Content = "The quick brown fox jumps over the lazy dog." });
            Assert.AreEqual("The quick brown fox jumps over the lazy dog.", bookPostInformation.Summary(200));
        }
    }
}
