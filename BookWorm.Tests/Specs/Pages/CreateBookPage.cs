using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace BookWorm.Tests.Specs.Pages
{
    public class CreateBookPage : BasePage
    {
        public bool IsCurrentPage()
        {
            return Driver.Title == "Add a Book";
        }

        public CreateBookPage FillForm(string title)
        {
            WaitForPageToLoad();
            SelectElement select;
            Driver.FindElement(By.Id("Model_Title")).SendKeys(title);
            Driver.FindElement(By.Id("Model_Subtitle")).SendKeys("ss");
            Driver.FindElement(By.Id("Model_Author")).SendKeys("ss");
            Driver.FindElement(By.Id("Model_Illustrator")).SendKeys("ss");
            Driver.FindElement(By.Id("Model_Editor")).SendKeys("ss");
            Driver.FindElement(By.Id("Model_Isbn")).SendKeys("123456789");
            Driver.FindElement(By.Id("Model_Publisher")).SendKeys("ss");
            select = new SelectElement(Driver.FindElement(By.Id("Model_Language")));
            select.SelectByText("Afrikaans");
            Driver.FindElement(By.Id("Model_Genre")).SendKeys("Fiction");
            select = new SelectElement(Driver.FindElement(By.Id("Model_AgeRange")));
            select.SelectByText("0-2 years");
            select = new SelectElement(Driver.FindElement(By.Id("Model_Country")));
            select.SelectByText("Tanzania");
            Driver.FindElement(By.Id("Model_CoverImageUrl")).SendKeys("http://images.google.co.za/intl/en_ALL/images/logos/images_logo_lg.gif");
            Driver.FindElement(By.Id("Model_Description")).SendKeys("ss");
            return  new CreateBookPage();
        }

        public BookDetailsPage ClickSaveButton()
        {
            Driver.FindElement(By.Id("SaveButton")).Click();
            WaitForPageToLoad();
            return new BookDetailsPage();
        }
    }
}
