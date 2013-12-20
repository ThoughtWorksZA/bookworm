using BookWorm.Models;
using BookWorm.ViewModels;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BookWorm.Tests.ViewModels
{
    [TestClass]
    public class BookInfromationTests
    {
        [TestMethod]
        public void SummaryShouldReturnBookDescriptionIfItsNull()
        {
            var bookInformation = new BookInformation
            {
                Model = new Book
                {
                    Description = null
                }
            };
            bookInformation.Summary(150).Should().Be(null);
        }
    }
}
