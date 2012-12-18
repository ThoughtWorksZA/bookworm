using OpenQA.Selenium;

namespace BookWorm.Tests.Specs.Pages
{
    public class BooksListPage
    {
        private static IWebDriver driver;

        public BooksListPage(IWebDriver webDriver)
        {
            driver = webDriver;
        }

        public bool IsCurrentPage()
        {
            return driver.Title == "List of Books - My ASP.NET MVC Application";
        }

        public CreateBookPage ClickOnCreateNewLink()
        {
            driver.FindElement(By.Id("createNewBookLink"));
            return new CreateBookPage(driver);
        }
    }
}
