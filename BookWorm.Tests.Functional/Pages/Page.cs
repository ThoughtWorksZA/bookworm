using BookWorm.Tests.Functional.Helpers;

namespace BookWorm.Tests.Functional.Pages
{
    public class Page
    {
        protected Browser Browser;

        public Page()
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
