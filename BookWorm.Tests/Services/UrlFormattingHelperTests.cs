using System;
using BookWorm.Services.Email;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BookWorm.Tests.Services
{
    [TestClass]
    public class UrlFormattingHelperTests
    {
        [TestMethod]
        public void ShouldCombineSchemeAndAuthority()
        {
            var helper = new UrlFormattingHelper();
            var baseUrl = helper.GetBaseUrl(new Uri("http://localhost:1234/soup"));
            baseUrl.Should().Be("http://localhost:1234");
        }
    }
}
