using OpenQA.Selenium;

namespace BookWorm.Tests.Specs.Pages
{
    public class RegisterPage : BasePage
    {
        private static IWebDriver driver;

        public RegisterPage(IWebDriver driver) : base(driver)
        {
        }

        public HomePage RegisterAdmin()
        {
            driver.FindElement(By.Id("UserName")).SendKeys("puku");
            driver.FindElement(By.Id("Password")).SendKeys("password");
            driver.FindElement(By.Id("ConfirmPassword")).SendKeys("password");
            driver.FindElement(By.Id("RegisterButton")).Click();
            return new HomePage(driver);
        } 
    }
}
