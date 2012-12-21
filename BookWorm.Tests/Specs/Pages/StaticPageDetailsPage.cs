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
            return Driver.Title == string.Format("{0} - My ASP.NET MVC Application", myPageTitle);
        }
    }
}