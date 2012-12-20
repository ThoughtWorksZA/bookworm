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
            return Driver.Title == "List of Books - My ASP.NET MVC Application";
        }

        public CreateBookPage ClickOnCreateNewLink()
        {
            Driver.FindElement(By.Id("createNewBookLink")).Click();
            return new CreateBookPage(Driver);
        }
    }
}
