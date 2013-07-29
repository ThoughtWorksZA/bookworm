using BookWorm.Models;
using BookWorm.Models.Validations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BookWorm.Tests.Models.Validations
{
    [TestClass]
    public class ValidRoleTest
    {
        [TestMethod]
        public void ShouldBeValidForAdminAndAuthor()
        {
            var validator = new ValidRole();
            Assert.IsTrue(validator.IsValid(Roles.Admin));
            Assert.IsTrue(validator.IsValid(Roles.Author));
        }
    
        [TestMethod]
        public void ShouldBeInvalidWhenSpecifiedEmptyOrOtherRoles ()
        {
            var validator = new ValidRole();
            Assert.IsFalse(validator.IsValid(" "));
            Assert.IsFalse(validator.IsValid("Select Role"));
        }
    }


}