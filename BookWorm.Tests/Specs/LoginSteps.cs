using BookWorm.Tests.Specs.Helpers;
using BookWorm.Tests.Specs.Pages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using TechTalk.SpecFlow;

namespace BookWorm.Tests.Specs
{
    [Binding]
    public class LoginSteps : Steps
    {
        private HomePage homePage;
        private LoginPage loginPage;
        private IWebDriver driver;

        [Given(@"I am on home page")]
        public void GivenIAmOnHomePage()
        {
            homePage = HomePage.NavigateTo(Browser.Driver);
        }

        [When(@"I click Login")]
        public void WhenIClickLogin()
        {
            loginPage = homePage.ClickOnLogin();

        }

        [Then(@"I see Login page")]
        public void ThenISeeLoginPage()
        {
            Assert.IsTrue(loginPage.IsCurrentPage());
        }
    }
}
