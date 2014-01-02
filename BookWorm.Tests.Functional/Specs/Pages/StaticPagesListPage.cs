using OpenQA.Selenium;

namespace BookWorm.Tests.Functional.Specs.Pages
{
    public class StaticPagesListPage : BasePage
    {
        public CreateStaticPage ClickOnCreateNewLink()
        {
            WaitForPageToLoad();
            Driver.FindElement(By.Id("createNewStaticPageLink")).Click();
            WaitForPageToLoad();
            return new CreateStaticPage();
        }
    }
}
