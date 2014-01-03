using OpenQA.Selenium;

namespace BookWorm.Tests.Functional.Pages
{
    public class BooksListPage : Page
    {
        public void ClickOnCreateNewLink()
        {
            Browser.FindElement(By.Id("createNewBookLink")).Click();
        }
    }
}
