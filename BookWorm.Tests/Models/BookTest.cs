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
        public void BookValidationsShouldBePresent()
        {

            var bookClass = typeof (Book);
            var languageProperty = bookClass.GetProperty("Language");
            Assert.IsNotNull(languageProperty);
            Assert.AreEqual(1, languageProperty.GetCustomAttributes(typeof(ValidLanguage), false).Count(), "The Language property of the Book Object should have a ValidLanguage annotation.");


        }
    }
}
