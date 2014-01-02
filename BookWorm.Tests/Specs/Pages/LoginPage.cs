using OpenQA.Selenium;

namespace BookWorm.Tests.Specs.Pages
{
    public class LoginPage : BasePage
    {
        public LoginPage(IWebDriver driver) : base(driver)
        {
        }
        
        public HomePage LoginAdmin()
        {
            WaitForPageToLoad();
            driver.FindElement(By.Id("Email")).SendKeys("puku@puku.co.za");
            driver.FindElement(By.Id("Password")).SendKeys("password");
            driver.FindElement(By.Id("LoginButton")).Click();
            return new HomePage(driver);
        }

        public bool IsCurrentPage()
        {
            return driver.Title == "Log in";
        }
    }
}
