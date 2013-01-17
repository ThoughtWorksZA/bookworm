using BookWorm.Models;

namespace BookWorm.ViewModels
{
    public class BookPostInformation
    {
        public int BookId { get; set; }
        public BookPost BookPost { get; set; }
        public Book Book { get; set; }

        public BookPostInformation(int bookId, BookPost bookPost)
        {
            BookId = bookId;
            BookPost = bookPost;
        }

        public BookPostInformation(int bookId, BookPost bookPost, Book book)
        {
            BookId = bookId;
            BookPost = bookPost;
            Book = book;
        }

        public string Summary(int characters)
        {
            if (BookPost.Content == null || BookPost.Content.Length < characters)
                return BookPost.Content;
            return BookPost.Content.Substring(0, BookPost.Content.IndexOf(" ", characters));
        }
    }
}