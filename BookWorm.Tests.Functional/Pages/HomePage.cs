using System.Configuration;
using OpenQA.Selenium;

namespace BookWorm.Tests.Functional.Pages
{
    public class HomePage : BasePage
    {
        public HomePage NavigateTo()
        {
            Browser.GoToUrl(ConfigurationManager.AppSettings["FunctionalTestURL"]);
            return new HomePage();
        }

        public LoginPage ClickOnLogin()
        {
            Browser.WaitAndFindElement(By.Id("loginLink")).Click();
            return new LoginPage();
        }

        public RegisterPage ClickOnRegister()
        {
            Browser.FindElement(By.Id("registerLink")).Click();
            return new RegisterPage();
        }

        public CreateBookPage NavigateToCreateBookPage()
        {
            Browser.WaitAndFindElement(By.Id("BooksMenuLink")).Click();
            return new BooksListPage().ClickOnCreateNewLink();
        }

        public CreateStaticPagePage NavigateToCreateStaticPageView()
        {
            var moreMenuLink = Browser.WaitAndFindElement(By.Id("moreMenuLink"));
            moreMenuLink.Click();
            var pagesMenuLinkElement = Browser.WaitAndFindElement(By.Id("PagesMenuLink"));
            pagesMenuLinkElement.Click();
            return new StaticPagesListPage().ClickOnCreateNewLink();
        }

        public HomePage LogOut()
        {
            Browser.FindElement(By.Id("LogOutLink")).Click();
            return new HomePage();
        }
    }
}
