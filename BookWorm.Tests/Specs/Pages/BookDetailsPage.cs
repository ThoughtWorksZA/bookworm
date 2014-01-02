namespace BookWorm.Tests.Specs.Pages
{
    public class BookDetailsPage :BasePage
    {
        public bool IsCurrentPage(string title)
        {
            return Driver.Title == title;
        }
    }
}
