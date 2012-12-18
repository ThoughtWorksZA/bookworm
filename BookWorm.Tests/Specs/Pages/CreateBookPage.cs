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

        public bool IsCurrentPage()
        {
            return driver.Title == "Add a Book - My ASP.NET MVC Application";
        }
    }
}
