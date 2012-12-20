using OpenQA.Selenium;

namespace BookWorm.Tests.Specs.Pages
{
    public class RegisterPage
    {
        private static IWebDriver driver;

        public RegisterPage(IWebDriver webDriver)
        {
            driver = webDriver;
        }

        public HomePage RegisterAdmin(IWebDriver webDriver)
        {
            webDriver.FindElement(By.Id("UserName")).SendKeys("puku");
            webDriver.FindElement(By.Id("Password")).SendKeys("password");
            webDriver.FindElement(By.Id("ConfirmPassword")).SendKeys("password");
            webDriver.FindElement(By.Id("RegisterButton")).Click();
            return new HomePage(webDriver);
        } 
    }
}
