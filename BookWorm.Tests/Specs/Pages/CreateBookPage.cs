using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace BookWorm.Tests.Specs.Pages
{
    public class CreateBookPage : BasePage
    {

        public CreateBookPage(IWebDriver driver) : base(driver)
        {
        }

        public bool IsCurrentPage()
        {
            return driver.Title == "Add a Book";
        }

        public CreateBookPage FillForm(string title)
        {
            WaitForPageToLoad();
            SelectElement select;
            driver.FindElement(By.Id("Model_Title")).SendKeys(title);
            driver.FindElement(By.Id("Model_Subtitle")).SendKeys("ss");
            driver.FindElement(By.Id("Model_Author")).SendKeys("ss");
            driver.FindElement(By.Id("Model_Illustrator")).SendKeys("ss");
            driver.FindElement(By.Id("Model_Editor")).SendKeys("ss");
            driver.FindElement(By.Id("Model_Isbn")).SendKeys("123456789");
            driver.FindElement(By.Id("Model_Publisher")).SendKeys("ss");
            select = new SelectElement(driver.FindElement(By.Id("Model_Language")));
            select.SelectByText("Afrikaans");
            driver.FindElement(By.Id("Model_Genre")).SendKeys("Fiction");
            select = new SelectElement(driver.FindElement(By.Id("Model_AgeRange")));
            select.SelectByText("0-2 years");
            select = new SelectElement(driver.FindElement(By.Id("Model_Country")));
            select.SelectByText("Tanzania");
            driver.FindElement(By.Id("Model_CoverImageUrl")).SendKeys("http://images.google.co.za/intl/en_ALL/images/logos/images_logo_lg.gif");
            driver.FindElement(By.Id("Model_Description")).SendKeys("ss");
            return  new CreateBookPage(driver);
        }

        public BookDetailsPage ClickSaveButton()
        {
            driver.FindElement(By.Id("SaveButton")).Click();
            WaitForPageToLoad();
            return new BookDetailsPage(driver);
        }
    }
}
