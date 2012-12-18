using System.Threading;
using OpenQA.Selenium;

namespace BookWorm.Tests.Specs.Pages
{
    public class HomePage
    {
        private static IWebDriver driver;

        public HomePage(IWebDriver webDriver)
        {
            driver = webDriver;
        }

        public static HomePage NavigateTo(IWebDriver webDriver)
        {
            webDriver.Navigate().GoToUrl("http://localhost:10827/");
            return new HomePage(webDriver);
        }

        public LoginPage ClickOnLogin()
        {
            driver.FindElement(By.Id("loginLink")).Click();
            return new LoginPage(driver);
        }


        public RegisterPage ClickOnRegister()
        {
            driver.FindElement(By.Id("registerLink")).Click();
            return new RegisterPage(driver);
        }

        public CreateBookPage NavigateToCreateBookPage()
        {
            driver.Navigate().GoToUrl("http://localhost:10827/books/create");
            return new CreateBookPage(driver);
        }
    }
}