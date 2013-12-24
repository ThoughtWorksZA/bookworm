using System;
using System.Configuration;
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
            webDriver.Navigate().GoToUrl(ConfigurationManager.AppSettings["FunctionalTestURL"]);
            return new HomePage(webDriver);
        }

        public LoginPage ClickOnLogin()
        {
            WaitForPageToLoad();
            driver.FindElement(By.Id("loginLink")).Click();
            WaitForPageToLoad();
            return new LoginPage(driver);
        }


        public RegisterPage ClickOnRegister()
        {
            driver.FindElement(By.Id("registerLink")).Click();
            return new RegisterPage(driver);
        }

        public CreateBookPage NavigateToCreateBookPage()
        {
            WaitForPageToLoad();
            driver.FindElement(By.Id("BooksMenuLink")).Click();
            WaitForPageToLoad();
            return new BooksListPage(driver).ClickOnCreateNewLink();
        }

        public CreateStaticPage NavigateToCreateStaticPageView()
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            var moreMenuLinkElement = wait.Until(d => d.FindElement(By.Id("moreMenuLink")));
            moreMenuLinkElement.Click();
            var pagesMenuLinkElement = wait.Until(d => d.FindElement(By.Id("PagesMenuLink")));
            pagesMenuLinkElement.Click();
            return new StaticPagesListPage(driver).ClickOnCreateNewLink();
        }

        public HomePage LogOut()
        {
            driver.FindElement(By.Id("LogOutLink")).Click();
            return new HomePage(driver);
        }
    }

   
}