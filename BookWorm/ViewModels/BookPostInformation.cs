using BookWorm.Models;

namespace BookWorm.ViewModels
{
    public class BookPostInformation
    {
        public int BookId { get; set; }
        public BookPost BookPost { get; set; }
        public Book Book { get; set; }

        public BookPostInformation()
        {
        }

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
            var md = new MarkdownSharp.Markdown();
            if (BookPost.Content == null || BookPost.Content.Length < characters)
                return md.Transform(BookPost.Content);
            return md.Transform(BookPost.Content.Substring(0, BookPost.Content.IndexOf(" ", characters)));
        }
    }
}