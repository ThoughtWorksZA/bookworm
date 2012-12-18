using BookWorm.Tests.Specs.Pages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using TechTalk.SpecFlow;

namespace BookWorm.Tests.Specs
{
    [Binding]
    internal class CreateBookSteps
    {
        private IWebDriver driver;
        private HomePage homePage;
        private CreateBookPage createBookPage;
        private BookDetailsPage bookDetailsPage;

        [BeforeScenario]
        public void Setup()
        {
            driver = new FirefoxDriver();
        }

        [Given(@"I am logged in as an admin")]
        public void IAmLoggedInAsAnAdmin()
        {
            homePage = HomePage.NavigateTo(driver);
            homePage = homePage.ClickOnLogin().LoginAdmin();
        }
        
        [When(@"I go to Create New Book page")]
        public void IGoToCreateNewBookPage()
        {
            createBookPage = homePage.NavigateToCreateBookPage();
        }

        [Then(@"I see Create New Book page")]
        public void ISeeCreateNewBookPage()
        {
            Assert.IsTrue(createBookPage.IsCurrentPage());
            homePage.LogOut();
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
            homePage.LogOut();
        }

        [Given(@"I am on Create New Book page")]
        public void IAmOnCreateNewBookPage()
        {
            IAmLoggedInAsAnAdmin();
            IGoToCreateNewBookPage();
        }

    [AfterScenario]
        public void TearDown()
        {
            driver.Quit();
        }
    }
}