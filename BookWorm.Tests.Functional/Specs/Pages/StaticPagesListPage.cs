﻿using OpenQA.Selenium;

namespace BookWorm.Tests.Functional.Specs.Pages
{
    public class StaticPagesListPage : BasePage
    {
        public CreateStaticPage ClickOnCreateNewLink()
        {
            Browser.WaitForPageToLoad();
            Browser.FindElement(By.Id("createNewStaticPageLink")).Click();
            Browser.WaitForPageToLoad();
            return new CreateStaticPage();
        }
    }
}
