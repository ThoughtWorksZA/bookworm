using System.Security.Principal;
using BookWorm.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BookWorm.Tests.Helpers
{
    [TestClass]
    public class IPrincipalExtensionTest
    {
        [TestMethod]
        public void ShouldCheckIsInRoles()
        {
            var mock = new Mock<IPrincipal>();
            mock.Setup(p => p.IsInRole("admin")).Returns(true);
            var principal = mock.Object;
            Assert.IsTrue(principal.IsInRoles("admin", "author"));
            Assert.IsFalse(principal.IsInRoles("normal", "author"));
        }
    }
}
