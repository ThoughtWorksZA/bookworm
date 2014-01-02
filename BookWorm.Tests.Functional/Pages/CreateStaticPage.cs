using OpenQA.Selenium;

namespace BookWorm.Tests.Functional.Pages
{
    public class CreateStaticPage : BasePage
    {
        public bool IsCurrentPage()
        {
            return Browser.Title == "PUKU | Add a Page";
        }

        public CreateStaticPage FillForm(string myPageTitle)
        {
            Browser.WaitForPageToLoad();
            Browser.FindElement(By.Name("Model.Title")).SendKeys(myPageTitle);
            Browser.FindElement(By.Name("Model.Content")).SendKeys("This is my content");
            return new CreateStaticPage();
        }

        public StaticPageDetailsPage ClickSaveButton()
        {
            Browser.FindElement(By.Id("SaveButton")).Click();
            Browser.WaitForPageToLoad();
            return new StaticPageDetailsPage();
        }
    }
}
