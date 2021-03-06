﻿using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TechTalk.SpecFlow;

namespace BookWorm.Tests.Functional.Helpers
{
    [Binding]
    public static class FeatureCoordinator
    {
        [BeforeFeature]
        public static void BeforeFeature()
        {
            if (ConfigurationManager.AppSettings["Environment"] == "Test")
                Assert.Inconclusive("Skipping test on AppHarbor");
            Database.SetupDocumentStore();
        }

        [BeforeScenario]
        public static void Setup()
        {
            Database.ClearDatabase();
            Database.CreateAdminUser();
            ScenarioContext.Current.Set<IWebDriver>(new ChromeDriver());
        }

        [AfterScenario]
        public static void TearDown()
        {
            ScenarioContext.Current.Get<IWebDriver>().Quit();
        }
    }
}
