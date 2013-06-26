using BookWorm.Helpers;
using BookWorm.Models;

namespace BookWorm.ViewModels
{
    public class BookPostInformation : ViewModel<BookPost>, IBasePostInformation<BookPost>
    {
        public int BookId { get; set; }
        public override BookPost Model { get; set; }
        public Book Book { get; set; }
        BasePost IBasePostInformation.Model { get { return Model; } }
        public string FeaturedImage { get { return Book.CoverImageUrl; } }

        public BookPostInformation()
        {
        }

        public BookPostInformation(int bookId, BookPost bookPost)
        {
            BookId = bookId;
            Model = bookPost;
        }

        public BookPostInformation(int bookId, BookPost bookPost, Book book)
        {
            BookId = bookId;
            Model = bookPost;
            Book = book;
        }

        public string Summary(int characters)
        {
            return MarkDownHelper.MarkDownSummary(Model.Content, characters);
        }

        public string DetailsUrl { get { return string.Format("/Books/Details/{0}", Book.Id); } }
    }
}