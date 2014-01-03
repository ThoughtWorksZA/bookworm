using BookWorm.Tests.Functional.Pages;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace BookWorm.Tests.Functional.Steps
{
    [Binding]
    internal class CreateBookSteps : TechTalk.SpecFlow.Steps
    {
        [Given(@"I am logged in as an admin")]
        public void IAmLoggedInAsAnAdmin()
        {
            Given("I navigate to the login page");
            When("I enter my credentials");
        }

        [When(@"I go to Create New Book page")]
        public void IGoToCreateNewBookPage()
        {
            var homePage = new HomePage();
            homePage.NavigateToCreateBookPage();
        }

        [When(@"I click create after filling the form")]
        public void IClickSaveAfterFillingTheForm()
        {
            var createBookPage = new CreateBookPage();
            createBookPage.FillInNewBookForm("My new title");
            createBookPage.ClickSaveButton();
        }

        [Then(@"I see the details of the newly created book")]
        public void ISeeTheDetailsOfTheNewlyCreatedBook()
        {
            var bookDetailsPage = new BookDetailsPage();
            bookDetailsPage.Title.Should().Be("My new title");
        }
    }
}
