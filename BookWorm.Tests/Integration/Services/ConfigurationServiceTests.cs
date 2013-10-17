using BookWorm.Services;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BookWorm.Tests.Integration.Services
{
    [TestClass]
    public class ConfigurationServiceTests
    {
        [TestMethod]
        public void ShouldFetchEmailSenderAddressFromWebConfig()
        {
            var configService = new ConfigurationService();
            configService.GetEmailSenderAddress().Should().Be("test-address-value");
        }
        
        [TestMethod]
        public void ShouldFetchEmailSenderPasswordFromWebConfig()
        {
            var configService = new ConfigurationService();
            configService.GetEmailSenderPassword().Should().Be("test-password-value");
        }
    }
}
