using OpenQA.Selenium;

namespace BookWorm.Tests.Functional.Pages
{
    public class StaticPagesListPage : BasePage
    {
        public CreateStaticPagePage ClickOnCreateNewLink()
        {
            Browser.WaitAndFindElement(By.Id("createNewStaticPageLink")).Click();
            return new CreateStaticPagePage();
        }
    }
}
