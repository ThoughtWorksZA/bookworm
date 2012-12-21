using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace BookWorm.Tests.Specs.Pages
{
    public class CreateStaticPageView : BasePage
    {
        public CreateStaticPageView(IWebDriver driver) : base(driver)
        {
        }

        public bool IsCurrentPage()
        {
            return Driver.Title == "PUKU | Add a Page - My ASP.NET MVC Application";
        }

        public CreateStaticPageView FillForm(string myPageTitle)
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            var titleElement = wait.Until(d => d.FindElement(By.Id("Title")));
            titleElement.SendKeys(myPageTitle);
            Driver.FindElement(By.Name("Content")).SendKeys("This is my content");
            return new CreateStaticPageView(Driver);
        }

        public StaticPageDetailsPage ClickSaveButton()
        {
            Driver.FindElement(By.Id("SaveButton")).Click();
            return new StaticPageDetailsPage(Driver);
        }
    }
}