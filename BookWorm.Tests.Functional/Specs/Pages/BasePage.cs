using BookWorm.Tests.Functional.Specs.Helpers;

namespace BookWorm.Tests.Functional.Specs.Pages
{
    public class BasePage
    {
        protected Browser Browser;

        public BasePage()
        {
            Browser = new Browser();
        }
    }
}
