using BookWorm.Models;
using BookWorm.ViewModels;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BookWorm.Tests.ViewModels
{
    [TestClass]
    public class BookInformationTests
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

        [TestMethod]
        public void SummaryShouldReturnBookDescriptionWhenItsShorterThanTheSummaryLengthRequested()
        {
            var bookInformation = new BookInformation
            {
                Model = new Book
                {
                    Description = "four"
                }
            };
            bookInformation.Summary(5).Should().Be("four");
        }

        [TestMethod]
        public void SummaryShouldReturnTheBookDescriptionUpToTheFirstSpaceAfterTheCharacterLimitRequested()
        {
            var bookInformation = new BookInformation
            {
                Model = new Book
                {
                    Description = "word word"
                }
            };
            bookInformation.Summary(2).Should().Be("word");
        }

        [TestMethod]
        public void SummaryShouldReturnTheBookDescriptionTruncatedToTheCharacterLimitIfThereAreNoSpacesAfterTheLimit()
        {
            var bookInformation = new BookInformation
            {
                Model = new Book
                {
                    Description = "wordword"
                }
            };
            bookInformation.Summary(4).Should().Be("word");
        }
    }
}
