using System;
using System.Collections.Generic;
using BookWorm.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PagedList;

namespace BookWorm.Tests.ViewModels
{
    [TestClass]
    public class FilterInformationTest
    {
        [TestMethod]
        public void ShouldBuildUrlForEmptyFilter()
        {
            var filterInformation = new FilterInformation(new List<BookInformation>().ToPagedList(1, 9));
            Assert.AreEqual("/Books/Filter?languages=Zulu", filterInformation.LanguageFilterUrl("Zulu"));
        }

        [TestMethod]
        public void ShouldBuildUrlForLanguageNotAlreadyInLanguageFilter()
        {
            var filterInformation = new FilterInformation(new List<string> { "Xhosa" }, new List<string>(), new List<string>(), new List<BookInformation>().ToPagedList(1, 9));
            Assert.AreEqual("/Books/Filter?languages=Zulu&languages=Xhosa", filterInformation.LanguageFilterUrl("Zulu"));
        }

        [TestMethod]
        public void ShouldBuildUrlForLanguageAlreadyInLanguageFilter()
        {
            var filterInformation = new FilterInformation(new List<string> { "Zulu", "Xhosa" }, new List<string>(), new List<string>(), new List<BookInformation>().ToPagedList(1, 9));
            Assert.AreEqual("/Books/Filter?languages=Xhosa", filterInformation.LanguageFilterUrl("Zulu"));
        }

        [TestMethod]
        public void ShouldBuildUrlForAgeRangeNotAlreadyInAgeRangeFilter()
        {
            var filterInformation = new FilterInformation(new List<string> { "Zulu", "Xhosa" }, new List<string> { "0-2" }, new List<string>(), new List<BookInformation>().ToPagedList(1, 9));
            Assert.AreEqual("/Books/Filter?ageRanges=3-6&ageRanges=0-2&languages=Zulu&languages=Xhosa", filterInformation.AgeRangeFilterUrl("3-6"));
        }

        [TestMethod]
        public void ShouldBuildUrlForAgeRangeAlreadyInAgeRangeFilter()
        {
            var filterInformation = new FilterInformation(new List<string> { "Zulu", "Xhosa" }, new List<string> { "0-2", "3-6" }, new List<string>(), new List<BookInformation>().ToPagedList(1, 9));
            Assert.AreEqual("/Books/Filter?ageRanges=0-2&languages=Zulu&languages=Xhosa", filterInformation.AgeRangeFilterUrl("3-6"));
        }

        [TestMethod]
        public void ShouldBuildUrlForGenreNotAlreadyInGenreFilter()
        {
            var filterInformation = new FilterInformation(new List<string> { "Zulu", "Xhosa" }, new List<string> { "0-2", "3-6" }, new List<string> { "Fiction" }, new List<BookInformation>().ToPagedList(1, 9));
            Assert.AreEqual("/Books/Filter?genres=Poetry&genres=Fiction&languages=Zulu&languages=Xhosa&ageRanges=0-2&ageRanges=3-6", filterInformation.GenreFilterUrl("Poetry"));
        }

        [TestMethod]
        public void ShouldBuildUrlForGenreAlreadyInGenreFilter()
        {
            var filterInformation = new FilterInformation(new List<string> { "Zulu", "Xhosa" }, new List<string> { "0-2", "3-6" }, new List<string> { "Fiction", "Poetry" }, new List<BookInformation>().ToPagedList(1, 9));
            Assert.AreEqual("/Books/Filter?genres=Fiction&languages=Zulu&languages=Xhosa&ageRanges=0-2&ageRanges=3-6", filterInformation.GenreFilterUrl("Poetry"));
        }

        [TestMethod]
        public void ShouldBuildUrlForPaginationWithFilters()
        {
            var filterInformation = new FilterInformation(new List<string> { "Zulu", "Xhosa" }, new List<string> { "0-2", "3-6" }, new List<string> { "Fiction", "Poetry" }, new List<BookInformation>().ToPagedList(1, 9));
            Assert.AreEqual("/Books/Filter?page=2&genres=Fiction&genres=Poetry&languages=Zulu&languages=Xhosa&ageRanges=0-2&ageRanges=3-6", filterInformation.PaginationUrl("Filter", 2));
        }

        [TestMethod]
        public void ShouldBuildUrlForPaginationWhenFiltersAreEmpty()
        {
            var filterInformation = new FilterInformation(new List<BookInformation>().ToPagedList(1, 9));
            Assert.AreEqual("/Books/List?page=2", filterInformation.PaginationUrl("List", 2));
        }
    }
}
