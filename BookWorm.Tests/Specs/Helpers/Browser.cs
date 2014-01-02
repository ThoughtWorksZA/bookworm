using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TechTalk.SpecFlow;

namespace BookWorm.Tests.Specs.Helpers
{
    [Binding]
    public static class Browser
    {
        public static IWebDriver Driver
        {
            get
            {
                return ScenarioContext.Current.Get<IWebDriver>();
            }
            set
            {
                ScenarioContext.Current.Set(value);
            }
        }

        [BeforeFeature]
        public static void BeforeFeature()
        {
            if (ConfigurationManager.AppSettings["Environment"] == "Test")
                Assert.Inconclusive("Skipping test on AppHarbor");
        }

        [BeforeScenario]
        public static void Setup()
        {
            Driver = new ChromeDriver();
        }

        [AfterScenario]
        public static void TearDown()
        {
            Driver.Quit();
        }
    }
}
