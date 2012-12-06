using BookWorm.Repository;

namespace BookWorm.Models
{
    public class Book : Model<Book>
    {
        public string Title { get; set; }
    }
}