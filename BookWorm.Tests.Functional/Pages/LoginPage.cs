using BookWorm.Tests.Functional.Helpers;
using OpenQA.Selenium;

namespace BookWorm.Tests.Functional.Pages
{
    public class LoginPage : Page
    {
        public void LoginAsAdmin()
        {
            Browser.WaitAndFindElement(By.Id("Email")).SendKeys(Users.AdminUserName);
            Browser.FindElement(By.Id("Password")).SendKeys("password");
            Browser.FindElement(By.Id("LoginButton")).Click();
        }

        public string WelcomeMessage
        {
            get
            {
                return Browser.FindElement(By.Id("welcomeInfo")).Text;
            }
        }
    }
}
