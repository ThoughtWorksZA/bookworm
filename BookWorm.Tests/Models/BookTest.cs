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

    }
}