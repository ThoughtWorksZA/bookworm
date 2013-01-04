using System;
using BookWorm.Models.Validations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BookWorm.Tests.Models.Validations
{
    [TestClass]
    public class ValidAgeGroupTest
    {
        [TestMethod]
        public void ShouldBeValidWhenSpecifiedAgeGroupIs0To2()
        {

            var validator = new ValidAgeRange();

            Assert.IsTrue(validator.IsValid("0-2"));
        }
    
        [TestMethod]
        public void ShouldBeInvalidWhenSpecifiedCountryIs1To3()
        {

            var validator = new ValidAgeRange();

            Assert.IsFalse(validator.IsValid("1-3"));
        }
    }

    
}
