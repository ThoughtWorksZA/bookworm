using System;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace BookWorm.Tests.Specs.Pages
{
    public class LoginPage {
    private static IWebDriver driver;

        public LoginPage(IWebDriver webDriver)
        {
            driver = webDriver;
        }
        
        public HomePage LoginAdmin()
        {
            driver.FindElement(By.Id("UserName")).SendKeys("puku");
            driver.FindElement(By.Id("Password")).SendKeys("password");
            driver.FindElement(By.Id("LoginButton")).Click();
             
            return new HomePage(driver);
        }

        public bool IsCurrentPage()
        {
            return driver.Title == "Log in - My ASP.NET MVC Application";
        }

    }

}
