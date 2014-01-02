using System;
using System.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace BookWorm.Tests.Functional.Specs.Pages
{
    public class HomePage : BasePage
    {
        public static HomePage NavigateTo(IWebDriver webDriver)
        {
            webDriver.Navigate().GoToUrl(ConfigurationManager.AppSettings["FunctionalTestURL"]);
            return new HomePage();
        }

        public LoginPage ClickOnLogin()
        {
            WaitForPageToLoad();
            Driver.FindElement(By.Id("loginLink")).Click();
            WaitForPageToLoad();
            return new LoginPage();
        }


        public RegisterPage ClickOnRegister()
        {
            Driver.FindElement(By.Id("registerLink")).Click();
            return new RegisterPage();
        }

        public CreateBookPage NavigateToCreateBookPage()
        {
            WaitForPageToLoad();
            Driver.FindElement(By.Id("BooksMenuLink")).Click();
            WaitForPageToLoad();
            return new BooksListPage().ClickOnCreateNewLink();
        }

        public CreateStaticPage NavigateToCreateStaticPageView()
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            var moreMenuLinkElement = wait.Until(d => d.FindElement(By.Id("moreMenuLink")));
            moreMenuLinkElement.Click();
            var pagesMenuLinkElement = wait.Until(d => d.FindElement(By.Id("PagesMenuLink")));
            pagesMenuLinkElement.Click();
            return new StaticPagesListPage().ClickOnCreateNewLink();
        }

        public HomePage LogOut()
        {
            Driver.FindElement(By.Id("LogOutLink")).Click();
            return new HomePage();
        }
    }

   
}