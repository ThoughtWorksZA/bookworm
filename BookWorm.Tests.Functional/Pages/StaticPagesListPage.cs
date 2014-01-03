using OpenQA.Selenium;

namespace BookWorm.Tests.Functional.Pages
{
    public class StaticPagesListPage : Page
    {
        public void ClickOnCreateNewLink()
        {
            Browser.WaitAndFindElement(By.Id("createNewStaticPageLink")).Click();
        }
    }
}
