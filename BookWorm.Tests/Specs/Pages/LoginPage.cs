using System;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace BookWorm.Tests.Specs.Pages
{
    public class LoginPage : BasePage
    {
        public LoginPage(IWebDriver driver) : base(driver)
        {
        }
    
        
        public HomePage LoginAdmin()
        {
            WaitForPageToLoad();
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
