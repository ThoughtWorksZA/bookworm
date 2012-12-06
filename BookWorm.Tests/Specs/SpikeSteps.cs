using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BookWorm.Tests.Specs.Pages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using TechTalk.SpecFlow;

namespace BookWorm.Tests.Specs
{
    [Binding]
    public class SpikeSteps
    {
        private HomePage homePage;
        private LoginPage loginPage;
        private IWebDriver driver;
        // For additional details on SpecFlow step definitions see http://go.specflow.org/doc-stepdef

        [BeforeScenario()]
        public void Setup()
        {
            driver = new FirefoxDriver();
        }

        [AfterScenario()]
        public void TearDown()
        {
            driver.Quit();
        }

        [Given(@"I am on home page")]
        public void GivenIAmOnHomePage()
        {
            homePage = HomePage.NavigateTo(driver);
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
