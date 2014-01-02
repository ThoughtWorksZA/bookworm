namespace BookWorm.Tests.Functional.Pages
{
    public class BookDetailsPage :BasePage
    {
        public bool IsCurrentPage(string title)
        {
            return Browser.Title == title;
        }
    }
}
