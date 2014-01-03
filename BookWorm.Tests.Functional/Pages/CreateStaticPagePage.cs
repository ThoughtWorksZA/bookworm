using OpenQA.Selenium;

namespace BookWorm.Tests.Functional.Pages
{
    public class CreateStaticPagePage : Page
    {
        public bool IsCurrentPage()
        {
            return Browser.Title == "PUKU | Add a Page";
        }

        public void FillForm(string myPageTitle)
        {
            Browser.WaitAndFindElement(By.Name("Model.Title")).SendKeys(myPageTitle);
            Browser.FindElement(By.Name("Model.Content")).SendKeys("This is my content");
        }

        public void ClickSaveButton()
        {
            Browser.FindElement(By.Id("SaveButton")).Click();
        }
    }
}
