using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using TechTalk.SpecFlow;

namespace BookWorm.Tests.Specs
{
    public class BaseSteps : Steps
    {
        protected IWebDriver Driver;

        [BeforeFeature]
        public static void BeforeFeature()
        {
            if (ConfigurationManager.AppSettings["Environment"] == "Test")
                Assert.Inconclusive("Skipping test on AppHarbor");
        }

        [BeforeScenario]
        public void Setup()
        {
            Driver = new ChromeDriver();
        }

        [AfterScenario]
        public void TearDown()
        {
            Driver.Quit();
        }
    }
}