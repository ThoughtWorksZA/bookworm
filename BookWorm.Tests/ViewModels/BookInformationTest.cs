using System;
using BookWorm.Models;
using BookWorm.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BookWorm.Tests.ViewModels
{
    [TestClass]
    public class BookInformationTest
    {
        [TestMethod]
        public void ShouldSummarizeDescription()
        {
            var bookInformation =
                new BookInformation(new Book {Description = "The quick brown fox jumps over the lazy dog."});
            Assert.AreEqual("The quick brown fox jumps", bookInformation.Summary(20));
        }

        [TestMethod]
        public void ShouldReturnDescriptionIfItSmallerThanTheSummarySize()
        {
            var bookInformation =
                new BookInformation(new Book { Description = "The quick brown fox jumps over the lazy dog." });
            Assert.AreEqual("The quick brown fox jumps over the lazy dog.", bookInformation.Summary(200));
        }
    }
}
