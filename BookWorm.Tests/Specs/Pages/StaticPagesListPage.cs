using System;
using OpenQA.Selenium;

namespace BookWorm.Tests.Specs.Pages
{
    public class StaticPagesListPage : BasePage
    {
        public StaticPagesListPage(IWebDriver driver) : base(driver)
        {
        }

        public CreateStaticPage ClickOnCreateNewLink()
        {
            WaitForPageToLoad();
            driver.FindElement(By.Id("createNewStaticPageLink")).Click();
            WaitForPageToLoad();
            return new CreateStaticPage(driver);
        }
    }
}