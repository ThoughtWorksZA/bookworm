using OpenQA.Selenium;

namespace BookWorm.Tests.Specs.Pages
{
    public class CreateStaticPageView : BasePage
    {
        public CreateStaticPageView(IWebDriver driver) : base(driver)
        {
        }

        public bool IsCurrentPage()
        {
            return Driver.Title == "PUKU | Add a Page - My ASP.NET MVC Application";
        }
    }
}