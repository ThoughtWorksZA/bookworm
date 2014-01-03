using BookWorm.Tests.Functional.Helpers;

namespace BookWorm.Tests.Functional.Pages
{
    public class BasePage
    {
        protected Browser Browser;

        public BasePage()
        {
            Browser = new Browser();
        }

        public string Title
        {
            get
            {
                return Browser.Title;
            }
        }
    }
}
