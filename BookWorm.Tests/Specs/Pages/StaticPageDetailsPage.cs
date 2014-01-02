namespace BookWorm.Tests.Specs.Pages
{
    public class StaticPageDetailsPage : BasePage
    {
        public bool IsCurrentPage(string myPageTitle)
        {
            return Driver.Title == myPageTitle;
        }
    }
}