using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace BookWorm.Tests.Specs.Pages
{
    public class CreateStaticPage : BasePage
    {
        public CreateStaticPage(IWebDriver driver) : base(driver)
        {
        }

        public bool IsCurrentPage()
        {
            return driver.Title == "PUKU | Add a Page - My ASP.NET MVC Application";
        }

        public CreateStaticPage FillForm(string myPageTitle)
        {
            WaitForPageToLoad();
            driver.FindElement(By.Id("Title")).SendKeys(myPageTitle);
            driver.FindElement(By.Name("Content")).SendKeys("This is my content");
            return new CreateStaticPage(driver);
        }

        public StaticPageDetailsPage ClickSaveButton()
        {
            driver.FindElement(By.Id("SaveButton")).Click();
            WaitForPageToLoad();
            return new StaticPageDetailsPage(driver);
        }
    }
}