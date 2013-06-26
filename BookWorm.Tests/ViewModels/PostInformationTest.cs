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
        public void ShouldReturnFeaturedImageUrlFromPost()
        {
            var postInformation =
               new PostInformation(new Post { FeaturedImage = "someurl"});
            Assert.AreEqual("someurl", postInformation.FeaturedImage);
        }
    }
}
