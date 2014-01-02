using OpenQA.Selenium;

namespace BookWorm.Tests.Specs.Pages
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
