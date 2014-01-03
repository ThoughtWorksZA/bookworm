using BookWorm.Tests.Functional.Helpers;
using BookWorm.Tests.Functional.Pages;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace BookWorm.Tests.Functional.Steps
{
    [Binding]
    public class LoginSteps : TechTalk.SpecFlow.Steps
    {
        [Given(@"I navigate to the login page")]
        public void GivenINavigateToTheLoginPage()
        {
            var homePage = new HomePage();
            homePage.NavigateTo();
            homePage.ClickOnLogin();
        }

        [When(@"I enter my credentials")]
        public void WhenIEnterMyCredentials()
        {
            var loginPage = new LoginPage();
            loginPage.LoginAsAdmin();
        }

        [Then(@"I see a welcome message")]
        public void ThenISeeAWelcomeMessage()
        {
            var loginPage = new LoginPage();
            loginPage.WelcomeMessage.Should().Be("Hello, " + Users.AdminUserName + "!");
        }
    }
}

