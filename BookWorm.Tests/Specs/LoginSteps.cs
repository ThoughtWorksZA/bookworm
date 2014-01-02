using BookWorm.Tests.Specs.Helpers;
using BookWorm.Tests.Specs.Pages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;

namespace BookWorm.Tests.Specs
{
    [Binding]
    public class LoginSteps : Steps
    {
        private HomePage _homePage;
        private LoginPage _loginPage;

        [Given(@"I am on home page")]
        public void GivenIAmOnHomePage()
        {
            _homePage = HomePage.NavigateTo(Browser.Driver);
        }

        [When(@"I click Login")]
        public void WhenIClickLogin()
        {
            _loginPage = _homePage.ClickOnLogin();
        }

        [Then(@"I see Login page")]
        public void ThenISeeLoginPage()
        {
            Assert.IsTrue(_loginPage.IsCurrentPage());
        }
    }
}
