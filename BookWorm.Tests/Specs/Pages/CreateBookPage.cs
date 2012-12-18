using OpenQA.Selenium;

namespace BookWorm.Tests.Specs.Pages
{
    public class CreateBookPage
    {
        private static IWebDriver driver;

        public CreateBookPage(IWebDriver webDriver)
        {
            driver = webDriver;
        }

        public string CurrentPage()
        {
            return driver.Title;
        }
    }
}
