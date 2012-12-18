using System;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace BookWorm.Tests.Specs.Pages
{
    public class HomePage
    {
        private static IWebDriver driver;

        public HomePage(IWebDriver webDriver)
        {
            driver = webDriver;
        }

        public static HomePage NavigateTo(IWebDriver webDriver)
        {
            webDriver.Navigate().GoToUrl("http://localhost:10827/");
            return new HomePage(webDriver);
        }

        public LoginPage ClickOnLogin()
        {
            driver.FindElement(By.Id("loginLink")).Click();
            return new LoginPage(driver);
        }


        public RegisterPage ClickOnRegister()
        {
            driver.FindElement(By.Id("registerLink")).Click();
            return new RegisterPage(driver);
        }

        public CreateBookPage NavigateToCreateBookPage()
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            var booksMenuLinkElement = wait.Until(d => d.FindElement(By.Id("BooksMenuLink")));
            booksMenuLinkElement.Click();
            return new BooksListPage(driver).ClickOnCreateNewLink();
        }

        public HomePage LogOut()
        {
            driver.FindElement(By.Id("LogOutLink")).Click();
            return new HomePage(driver);
        }
    }
}