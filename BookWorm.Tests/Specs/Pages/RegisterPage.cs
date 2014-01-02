using OpenQA.Selenium;

namespace BookWorm.Tests.Specs.Pages
{
    public class RegisterPage : BasePage
    {
        public HomePage RegisterAdmin()
        {
            Driver.FindElement(By.Id("UserName")).SendKeys("puku");
            Driver.FindElement(By.Id("Password")).SendKeys("password");
            Driver.FindElement(By.Id("ConfirmPassword")).SendKeys("password");
            Driver.FindElement(By.Id("RegisterButton")).Click();
            return new HomePage();
        } 
    }
}
