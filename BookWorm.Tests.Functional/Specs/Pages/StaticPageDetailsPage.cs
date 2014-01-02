namespace BookWorm.Tests.Functional.Specs.Pages
{
    public class StaticPageDetailsPage : BasePage
    {
        public bool IsCurrentPage(string myPageTitle)
        {
            return Browser.Title == myPageTitle;
        }
    }
}