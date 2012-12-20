using BookWorm.Tests.Specs.Pages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;

namespace BookWorm.Tests.Specs
{
    [Binding]
    internal class CreateBookSteps : BaseSteps
    {
        private CreateBookPage createBookPage;
        private BookDetailsPage bookDetailsPage;

        [Given(@"I am logged in as an admin")]
        public void IAmLoggedInAsAnAdmin()
        {
            HomePage = HomePage.NavigateTo(Driver);
            HomePage = HomePage.ClickOnLogin().LoginAdmin();
        }

        [When(@"I go to Create New Book page")]
        public void IGoToCreateNewBookPage()
        {
            createBookPage = HomePage.NavigateToCreateBookPage();
        }

        [Then(@"I see Create New Book page")]
        public void ISeeCreateNewBookPage()
        {
            Assert.IsTrue(createBookPage.IsCurrentPage());
            HomePage.LogOut();
        }

        [When(@"I click create after filling the form")]
        public void IClickSaveAfterFillingTheForm()
        {
            bookDetailsPage = createBookPage.FillForm("My new title").ClickSaveButton();
        }

        [Then(@"I see the details of the newly created book")]
        public void ISeeTheDetailsOfTheNewlyCreatedBook()
        {
            Assert.IsTrue(bookDetailsPage.IsCurrentPage("My new title"));
            HomePage.LogOut();
        }

        [Given(@"I am on Create New Book page")]
        public void IAmOnCreateNewBookPage()
        {
            IAmLoggedInAsAnAdmin();
            IGoToCreateNewBookPage();
        }
    }
}