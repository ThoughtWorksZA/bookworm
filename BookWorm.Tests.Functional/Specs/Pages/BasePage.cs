using System;
using BookWorm.Tests.Functional.Specs.Helpers;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace BookWorm.Tests.Functional.Specs.Pages
{
    public class BasePage
    {
        protected static IWebDriver Driver;

        public BasePage()
        {
            Driver = Browser.Driver;
        }

        public void WaitForPageToLoad()
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("body")));
        }
    }
}
