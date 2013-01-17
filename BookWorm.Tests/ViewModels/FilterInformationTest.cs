using System;
using System.Collections.Generic;
using BookWorm.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BookWorm.Tests.ViewModels
{
    [TestClass]
    public class FilterInformationTest
    {
        [TestMethod]
        public void ShouldBuildUrlForEmptyFilter()
        {
            var filterInformation = new FilterInformation(new List<BookInformation>());
            Assert.AreEqual("/Books/Filter?languages=Zulu", filterInformation.LanguageFilterUrl("Zulu"));
        }

        [TestMethod]
        public void ShouldBuildUrlForLanguageNotAlreadyInLanguageFilter()
        {
            var filterInformation = new FilterInformation(new List<string> {"Xhosa"}, new List<string>(), new List<BookInformation>());
            Assert.AreEqual("/Books/Filter?languages=Zulu&languages=Xhosa", filterInformation.LanguageFilterUrl("Zulu"));
        }

        [TestMethod]
        public void ShouldBuildUrlForLanguageAlreadyInLanguageFilter()
        {
            var filterInformation = new FilterInformation(new List<string> { "Zulu", "Xhosa" }, new List<string>(), new List<BookInformation>());
            Assert.AreEqual("/Books/Filter?languages=Xhosa", filterInformation.LanguageFilterUrl("Zulu"));
        }

        [TestMethod]
        public void ShouldBuildUrlForAgeRangeNotAlreadyInAgeRangeFilter()
        {
            var filterInformation = new FilterInformation(new List<string> { "Zulu", "Xhosa" }, new List<string> { "0-2" }, new List<BookInformation>());
            Assert.AreEqual("/Books/Filter?ageRanges=3-6&ageRanges=0-2&languages=Zulu&languages=Xhosa", filterInformation.AgeRangeFilterUrl("3-6"));
        }

        [TestMethod]
        public void ShouldBuildUrlForAgeRangeAlreadyInAgeRangeFilter()
        {
            var filterInformation = new FilterInformation(new List<string> { "Zulu", "Xhosa" }, new List<string> { "0-2", "3-6" }, new List<BookInformation>());
            Assert.AreEqual("/Books/Filter?ageRanges=0-2&languages=Zulu&languages=Xhosa", filterInformation.AgeRangeFilterUrl("3-6"));
        }
    }
}
