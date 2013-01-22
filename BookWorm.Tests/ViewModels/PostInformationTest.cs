using System;
using BookWorm.Models;
using BookWorm.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BookWorm.Tests.ViewModels
{
    [TestClass]
    public class PostInformationTest
    {
        [TestMethod]
        public void ShouldSummarizeDescription()
        {
            var postInformation =
                new PostInformation(new Post { Content = "__The quick brown fox__ jumps over the lazy dog." });
            Assert.AreEqual("<p><strong>The quick brown fox</strong> jumps</p>\n", postInformation.Summary(24));
        }

        [TestMethod]
        public void ShouldReturnDescriptionIfItSmallerThanTheSummarySize()
        {
            var postInformation =
                new PostInformation(new Post { Content = "The quick brown fox jumps over the lazy dog." });
            Assert.AreEqual("<p>The quick brown fox jumps over the lazy dog.</p>\n", postInformation.Summary(200));
        }

        [TestMethod]
        public void ShouldReturnFeaturedImageUrlFromPost()
        {
            var postInformation =
               new PostInformation(new Post { FeaturedImage = "someurl"});
            Assert.AreEqual("someurl", postInformation.FeaturedImage);
        }
    }
}
