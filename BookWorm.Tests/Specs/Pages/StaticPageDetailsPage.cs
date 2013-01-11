using OpenQA.Selenium;

namespace BookWorm.Tests.Specs.Pages
{
    public class StaticPageDetailsPage : BasePage
    {
        public StaticPageDetailsPage(IWebDriver driver) :base (driver)
        {
            
        }

        public bool IsCurrentPage(string myPageTitle)
        {
            return driver.Title == myPageTitle;
        }
    }
}