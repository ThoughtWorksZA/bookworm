using System.Configuration;
using OpenQA.Selenium;

namespace BookWorm.Tests.Functional.Pages
{
    public class HomePage : Page
    {
        public void NavigateTo()
        {
            Browser.GoToUrl(ConfigurationManager.AppSettings["FunctionalTestURL"]);
        }

        public void ClickOnLogin()
        {
            Browser.WaitAndFindElement(By.Id("loginLink")).Click();
        }

        public void ClickOnRegister()
        {
            Browser.FindElement(By.Id("registerLink")).Click();
        }

        public CreateBookPage NavigateToCreateBookPage()
        {
            Browser.WaitAndFindElement(By.Id("BooksMenuLink")).Click();
            return new BooksListPage().ClickOnCreateNewLink();
        }

        public void NavigateToCreateStaticPageView()
        {
            var moreMenuLink = Browser.WaitAndFindElement(By.Id("moreMenuLink"));
            moreMenuLink.Click();
            var pagesMenuLinkElement = Browser.WaitAndFindElement(By.Id("PagesMenuLink"));
            pagesMenuLinkElement.Click();
            new StaticPagesListPage().ClickOnCreateNewLink();
        }

        public void LogOut()
        {
            Browser.FindElement(By.Id("LogOutLink")).Click();
        }
    }
}
