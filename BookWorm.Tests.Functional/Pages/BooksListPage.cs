using OpenQA.Selenium;

namespace BookWorm.Tests.Functional.Pages
{
    public class BooksListPage : Page
    {
        public CreateBookPage ClickOnCreateNewLink()
        {
            Browser.FindElement(By.Id("createNewBookLink")).Click();
            return new CreateBookPage();
        }
    }
}
