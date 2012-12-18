using OpenQA.Selenium;

namespace BookWorm.Tests.Specs.Pages
{
    public class BookDetailsPage
    {
        private static IWebDriver driver;

        public BookDetailsPage(IWebDriver webDriver)
        {
            driver = webDriver;
        }

        public bool IsCurrentPage(string title)
        {
            return driver.Title == string.Format("{0} - My ASP.NET MVC Application", title);
        }

    }
}
