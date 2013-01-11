using OpenQA.Selenium;

namespace BookWorm.Tests.Specs.Pages
{
    public class BookDetailsPage :BasePage
    {

        public BookDetailsPage(IWebDriver driver) : base(driver)
        {
        }

        public bool IsCurrentPage(string title)
        {
            return driver.Title == title;
        }

    }
}
