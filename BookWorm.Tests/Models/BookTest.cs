using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookWorm.Models;
using BookWorm.Models.Validations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BookWorm.Tests.Models
{
    [TestClass]
    public class BookTest
    {
        
        [TestMethod]
        public void MandatoryPropertiesShouldBePresent()
        {
            var mandatoryProperties = new List<string>
                {
                    "Title",
                    "Author",
                    "Editor",
                    "Isbn",
                    "Publisher",
                    "Language",
                    "Genre",
                    "RecommendedAgeGroup",
                    "Country"
                };

            var bookClass = typeof (Book);

            foreach (var propertyName in mandatoryProperties)
            {
                var namedProperty = bookClass.GetProperty(propertyName);
                Assert.IsNotNull(namedProperty);
                Assert.AreEqual(1,
                                namedProperty.GetCustomAttributes(
                                    typeof (System.ComponentModel.DataAnnotations.RequiredAttribute), false).Count(),
                                string.Format(
                                    "{0} is a mandatory field, but 'Required' annotation was not found on the Book object.",
                                    propertyName));
            }
        }

        [TestMethod]
        public void LanguageShouldBeInValidLanguages()
        {
            var bookClass = typeof(Book);
            var propertyName = "Language";
            var namedProperty = bookClass.GetProperty(propertyName);
            Assert.IsNotNull(namedProperty);
            Assert.AreEqual(1, namedProperty.GetCustomAttributes(typeof(ValidLanguage), false).Count(),
                            "The Language property of the Book Object should have a ValidLanguage annotation.");
        }

        [TestMethod]
        public void CountryShouldBeInValidCountries()
        {
            var bookClass = typeof(Book);
            var propertyName = "Country";
            var namedProperty = bookClass.GetProperty(propertyName);
            Assert.IsNotNull(namedProperty);
            Assert.AreEqual(1, namedProperty.GetCustomAttributes(typeof(ValidCountry), false).Count(),
                            "The Country property of the Book Object should have a ValidCountry annotation.");
        }

        [TestMethod]
        public void AgeGroupShouldBeInValidAgeGroups()
        {
            var bookClass = typeof(Book);
            var propertyName = "RecommendedAgeGroup";
            var namedProperty = bookClass.GetProperty(propertyName);
            Assert.IsNotNull(namedProperty);
            Assert.AreEqual(1, namedProperty.GetCustomAttributes(typeof(ValidAgeGroup), false).Count(),
                            "The RecommendedAgeGroup property of the Book Object should have a ValidAgeGroup annotation.");
        }

        [TestMethod]
        public void ISBNShouldBe9To13CharactersLong()
        {
            var bookClass = typeof(Book);
            var propertyName = "Isbn";
            var namedProperty = bookClass.GetProperty(propertyName);
            Assert.IsNotNull(namedProperty);
            Assert.AreEqual(1, namedProperty.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.StringLengthAttribute), false).Count(),
                            "The Isbn property of the Book Object should have a StringLength annotation.");

            var annotation = (System.ComponentModel.DataAnnotations.StringLengthAttribute)namedProperty.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.StringLengthAttribute), false).First();
            
            Assert.AreEqual(13, annotation.MaximumLength);
            Assert.AreEqual(9, annotation.MinimumLength);
        }

        [TestMethod]
        public void ISBNShouldOnlyContainLettersAndDigits()
        {
            var bookClass = typeof(Book);
            var propertyName = "Isbn";
            var namedProperty = bookClass.GetProperty(propertyName);
            Assert.IsNotNull(namedProperty);
            var customAttributes = namedProperty.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.RegularExpressionAttribute), false);

            Assert.AreEqual(1, customAttributes.Count(),
                            "The Isbn property of the Book Object should have a RegularExpression annotation.");

            var annotation = (System.ComponentModel.DataAnnotations.RegularExpressionAttribute) customAttributes.First();
            Assert.AreEqual("^([a-zA-Z0-9]+)$", annotation.Pattern);
        }


    }
}