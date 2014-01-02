using OpenQA.Selenium;

namespace BookWorm.Tests.Functional.Specs.Pages
{
    public class BooksListPage : BasePage
    {
        public bool IsCurrentPage()
        {
            return Driver.Title == "List of Books";
        }

        public CreateBookPage ClickOnCreateNewLink()
        {
            Driver.FindElement(By.Id("createNewBookLink")).Click();
            WaitForPageToLoad();
            return new CreateBookPage();
        }
    }
}
