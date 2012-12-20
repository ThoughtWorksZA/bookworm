using System;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace BookWorm.Tests.Specs.Pages
{
    public class HomePage : BasePage
    {
        public HomePage(IWebDriver driver) : base(driver)
        {
        }

        public static HomePage NavigateTo(IWebDriver webDriver)
        {
            webDriver.Navigate().GoToUrl("http://localhost:10827/");
            return new HomePage(webDriver);
        }

        public LoginPage ClickOnLogin()
        {
            Driver.FindElement(By.Id("loginLink")).Click();
            return new LoginPage(Driver);
        }


        public RegisterPage ClickOnRegister()
        {
            Driver.FindElement(By.Id("registerLink")).Click();
            return new RegisterPage(Driver);
        }

        public CreateBookPage NavigateToCreateBookPage()
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            var booksMenuLinkElement = wait.Until(d => d.FindElement(By.Id("BooksMenuLink")));
            booksMenuLinkElement.Click();
            wait.Until(d => d.FindElement(By.Id("createNewBookLink")));
            return new BooksListPage(Driver).ClickOnCreateNewLink();
        }

        public CreateStaticPageView NavigateToCreateStaticPageView()
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            var pagesMenuLinkElement = wait.Until(d => d.FindElement(By.Id("PagesMenuLink")));
            pagesMenuLinkElement.Click();
            return new StaticPagesListPage(Driver).ClickOnCreateNewLink();
        }

        public HomePage LogOut()
        {
            Driver.FindElement(By.Id("LogOutLink")).Click();
            return new HomePage(Driver);
        }
    }

    public class BasePage
    {
        protected static IWebDriver Driver;

        public BasePage(IWebDriver driver)
        {
            Driver = driver;
        }
    }
}