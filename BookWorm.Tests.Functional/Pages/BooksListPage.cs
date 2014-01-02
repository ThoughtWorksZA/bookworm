using OpenQA.Selenium;

namespace BookWorm.Tests.Functional.Pages
{
    public class BooksListPage : BasePage
    {
        public bool IsCurrentPage()
        {
            return Browser.Title == "List of Books";
        }

        public CreateBookPage ClickOnCreateNewLink()
        {
            Browser.FindElement(By.Id("createNewBookLink")).Click();
            Browser.WaitForPageToLoad();
            return new CreateBookPage();
        }
    }
}
