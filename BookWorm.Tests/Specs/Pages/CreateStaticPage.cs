using OpenQA.Selenium;

namespace BookWorm.Tests.Specs.Pages
{
    public class CreateStaticPage : BasePage
    {
        public bool IsCurrentPage()
        {
            return Driver.Title == "PUKU | Add a Page";
        }

        public CreateStaticPage FillForm(string myPageTitle)
        {
            WaitForPageToLoad();
            Driver.FindElement(By.Name("Model.Title")).SendKeys(myPageTitle);
            Driver.FindElement(By.Name("Model.Content")).SendKeys("This is my content");
            return new CreateStaticPage();
        }

        public StaticPageDetailsPage ClickSaveButton()
        {
            Driver.FindElement(By.Id("SaveButton")).Click();
            WaitForPageToLoad();
            return new StaticPageDetailsPage();
        }
    }
}
