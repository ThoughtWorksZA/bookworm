using OpenQA.Selenium;

namespace BookWorm.Tests.Functional.Pages
{
    public class RegisterPage : BasePage
    {
        public HomePage RegisterAdmin()
        {
            Browser.FindElement(By.Id("UserName")).SendKeys("puku");
            Browser.FindElement(By.Id("Password")).SendKeys("password");
            Browser.FindElement(By.Id("ConfirmPassword")).SendKeys("password");
            Browser.FindElement(By.Id("RegisterButton")).Click();
            return new HomePage();
        } 
    }
}
