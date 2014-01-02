using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace BookWorm.Tests.Functional.Pages
{
    public class CreateBookPage : BasePage
    {
        public bool IsCurrentPage()
        {
            return Browser.Title == "Add a Book";
        }

        public CreateBookPage FillForm(string title)
        {
            Browser.WaitForPageToLoad();
            SelectElement select;
            Browser.FindElement(By.Id("Model_Title")).SendKeys(title);
            Browser.FindElement(By.Id("Model_Subtitle")).SendKeys("ss");
            Browser.FindElement(By.Id("Model_Author")).SendKeys("ss");
            Browser.FindElement(By.Id("Model_Illustrator")).SendKeys("ss");
            Browser.FindElement(By.Id("Model_Editor")).SendKeys("ss");
            Browser.FindElement(By.Id("Model_Isbn")).SendKeys("123456789");
            Browser.FindElement(By.Id("Model_Publisher")).SendKeys("ss");
            select = new SelectElement(Browser.FindElement(By.Id("Model_Language")));
            select.SelectByText("Afrikaans");
            Browser.FindElement(By.Id("Model_Genre")).SendKeys("Fiction");
            select = new SelectElement(Browser.FindElement(By.Id("Model_AgeRange")));
            select.SelectByText("0-2 years");
            select = new SelectElement(Browser.FindElement(By.Id("Model_Country")));
            select.SelectByText("Tanzania");
            Browser.FindElement(By.Id("Model_CoverImageUrl")).SendKeys("http://images.google.co.za/intl/en_ALL/images/logos/images_logo_lg.gif");
            Browser.FindElement(By.Id("Model_Description")).SendKeys("ss");
            return  new CreateBookPage();
        }

        public BookDetailsPage ClickSaveButton()
        {
            Browser.FindElement(By.Id("SaveButton")).Click();
            Browser.WaitForPageToLoad();
            return new BookDetailsPage();
        }
    }
}
