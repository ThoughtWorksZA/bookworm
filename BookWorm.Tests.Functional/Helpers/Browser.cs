using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using TechTalk.SpecFlow;

namespace BookWorm.Tests.Functional.Helpers
{
    public class Browser
    {
        private readonly IWebDriver _driver;

        public Browser()
        {
            _driver = ScenarioContext.Current.Get<IWebDriver>();
        }

        public string Title
        {
            get
            {
                return _driver.Title;
            }
        }

        public IWebElement FindElement(By @by)
        {
            return _driver.FindElement(@by);
        }

        public void GoToUrl(string url)
        {
            _driver.Navigate().GoToUrl(url);
        }

        public IWebElement WaitAndFindElement(By @by)
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(100));
            return wait.Until(it => it.FindElement(@by));
        }
    }
}
