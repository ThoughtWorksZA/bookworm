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

        [When(@"I go to Create New Book Page")]
        public void IGoToCreateNewBookPage()
        {
            createBookPage = homePage.NavigateToCreateBookPage();
        }

        [Then(@"I see Create New Book page")]
        public void ISeeCreateNewBookPage()
        {
            Assert.AreEqual("Add a Book - My ASP.NET MVC Application", createBookPage.CurrentPage());            
        }

    [AfterScenario]
        public void TearDown()
        {
            driver.Quit();
        }
    }
}