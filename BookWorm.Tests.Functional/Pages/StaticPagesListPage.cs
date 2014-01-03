using OpenQA.Selenium;

namespace BookWorm.Tests.Functional.Pages
{
    public class StaticPagesListPage : BasePage
    {
        public CreateStaticPagePage ClickOnCreateNewLink()
        {
            Browser.WaitForPageToLoad();
            Browser.FindElement(By.Id("createNewStaticPageLink")).Click();
            Browser.WaitForPageToLoad();
            return new CreateStaticPagePage();
        }
    }
}
