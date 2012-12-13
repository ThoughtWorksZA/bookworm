using System;
using BookWorm.Models.Validations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BookWorm.Tests.Models.Validations
{
    [TestClass]
    public class ValidCountryTest
    {
        [TestMethod]
        public void ShouldBeValidWhenSpecifiedCountryIsAngola()
        {

            var validator = new ValidCountry();

            Assert.IsTrue(validator.IsValid("Angola"));
        }
    
        [TestMethod]
        public void ShouldBeInvalidWhenSpecifiedCountryIsSudan()
        {

            var validator = new ValidCountry();

            Assert.IsFalse(validator.IsValid("Sudan"));
        }
    }

    
}
