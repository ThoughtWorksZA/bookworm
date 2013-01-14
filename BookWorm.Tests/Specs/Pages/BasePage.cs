using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace BookWorm.Tests.Specs.Pages
{
    public class BasePage
    {
        protected static IWebDriver driver;


        public BasePage(IWebDriver webDriver)
        {
            driver = webDriver;
        }

        public void WaitForPageToLoad()
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            wait.Until(d => d.FindElement(By.Id("body")));

        }
    }
}
