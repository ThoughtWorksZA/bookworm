using BookWorm.Tests.Functional.Pages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;

namespace BookWorm.Tests.Functional.Steps
{
    [Binding]
    internal class CreateBookSteps : TechTalk.SpecFlow.Steps
    {
        [Given(@"I am logged in as an admin")]
        public void IAmLoggedInAsAnAdmin()
        {
            var homePage = new HomePage();
            homePage.NavigateTo();
            homePage = homePage.ClickOnLogin().LoginAdmin();
            ScenarioContext.Current.Set(homePage);
        }

        [When(@"I go to Create New Book page")]
        public void IGoToCreateNewBookPage()
        {
            var homePage = ScenarioContext.Current.Get<HomePage>();
            var createBookPage = homePage.NavigateToCreateBookPage();
            ScenarioContext.Current.Set(createBookPage);
        }

        [When(@"I click create after filling the form")]
        public void IClickSaveAfterFillingTheForm()
        {
            var createBookPage = ScenarioContext.Current.Get<CreateBookPage>();
            var bookDetailsPage = createBookPage.FillForm("My new title").ClickSaveButton();
            ScenarioContext.Current.Set(bookDetailsPage);
        }

        [Then(@"I see the details of the newly created book")]
        public void ISeeTheDetailsOfTheNewlyCreatedBook()
        {
            var bookDetailsPage = ScenarioContext.Current.Get<BookDetailsPage>();
            var homePage = ScenarioContext.Current.Get<HomePage>();
            Assert.IsTrue(bookDetailsPage.IsCurrentPage("My new title"));
            homePage.LogOut();
        }
    }
}