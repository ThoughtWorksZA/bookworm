using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace BookWorm.Tests.Specs.Pages
{
    public class HomePage {
    private static IWebDriver driver;

        private HomePage(IWebDriver webDriver)
        {
            driver = webDriver;
        }

        public static HomePage NavigateTo(IWebDriver webDriver)
        {
            webDriver.Navigate().GoToUrl("http://localhost:10827/");
            var homePage = new HomePage(webDriver);
            return homePage;
        }

        public LoginPage ClickOnLogin()
        {
            driver.FindElement(By.Id("loginLink")).Click();
            var loginPage = new LoginPage(driver);
            return loginPage;
        }


    }

}
