using System;
using OpenQA.Selenium;

namespace BookWorm.Tests.Specs.Pages
{
    public class StaticPagesListPage : BasePage
    {
        public StaticPagesListPage(IWebDriver driver) : base(driver)
        {
        }

        public CreateStaticPageView ClickOnCreateNewLink()
        {
            Driver.FindElement(By.Id("createNewStaticPageLink")).Click();
            return new CreateStaticPageView(Driver);
        }
    }
}