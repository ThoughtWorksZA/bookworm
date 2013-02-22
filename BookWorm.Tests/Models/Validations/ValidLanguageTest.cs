using System;
using BookWorm.Models.Validations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BookWorm.Tests.Models.Validations
{
    [TestClass]
    public class ValidLanguageTest
    {
        [TestMethod]
        public void ShouldBeValidWhenSpecifiedLanguageIsZulu()
        {

            var validator = new ValidLanguage();

            Assert.IsTrue(validator.IsValid("isiZulu"));
        }
    
        [TestMethod]
        public void ShouldBeInvalidWhenSpecifiedLanguageIsFrench()
        {

            var validator = new ValidLanguage();

            Assert.IsFalse(validator.IsValid("French"));
        }
    }
}
