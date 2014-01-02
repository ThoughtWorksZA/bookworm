using OpenQA.Selenium;

namespace BookWorm.Tests.Functional.Pages
{
    public class LoginPage : BasePage
    {
        public HomePage LoginAdmin()
        {
            Browser.WaitForPageToLoad();
            Browser.FindElement(By.Id("Email")).SendKeys("puku@puku.co.za");
            Browser.FindElement(By.Id("Password")).SendKeys("password");
            Browser.FindElement(By.Id("LoginButton")).Click();
            return new HomePage();
        }

        public bool IsCurrentPage()
        {
            return Browser.Title == "Log in";
        }
    }
}
