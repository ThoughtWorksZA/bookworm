using BookWorm.Tests.Functional.Specs.Pages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;

namespace BookWorm.Tests.Functional.Specs
{
    [Binding]
    public class LoginSteps : Steps
    {
        private HomePage _homePage;
        private LoginPage _loginPage;

        [Given(@"I am on home page")]
        public void GivenIAmOnHomePage()
        {
            _homePage = new HomePage();
            _homePage.NavigateTo();
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
