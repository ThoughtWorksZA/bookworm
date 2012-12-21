using OpenQA.Selenium;

namespace BookWorm.Tests.Specs.Pages
{
    public class BooksListPage : BasePage
    {
        public BooksListPage(IWebDriver driver) : base(driver)
        {
        }

        public bool IsCurrentPage()
        {
            return driver.Title == "List of Books - My ASP.NET MVC Application";
        }

        public CreateBookPage ClickOnCreateNewLink()
        {
            driver.FindElement(By.Id("createNewBookLink")).Click();
            WaitForPageToLoad();
            return new CreateBookPage(driver);
        }
    }
}
