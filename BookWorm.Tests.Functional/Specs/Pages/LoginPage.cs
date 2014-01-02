using OpenQA.Selenium;

namespace BookWorm.Tests.Functional.Specs.Pages
{
    public class LoginPage : BasePage
    {
        public HomePage LoginAdmin()
        {
            WaitForPageToLoad();
            Driver.FindElement(By.Id("Email")).SendKeys("puku@puku.co.za");
            Driver.FindElement(By.Id("Password")).SendKeys("password");
            Driver.FindElement(By.Id("LoginButton")).Click();
            return new HomePage();
        }

        public bool IsCurrentPage()
        {
            return Driver.Title == "Log in";
        }
    }
}
