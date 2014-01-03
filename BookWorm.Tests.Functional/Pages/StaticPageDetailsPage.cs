namespace BookWorm.Tests.Functional.Pages
{
    public class StaticPageDetailsPage : BasePage
    {
        public bool IsCurrentPage(string myPageTitle)
        {
            return Browser.Title == myPageTitle;
        }
    }
}
