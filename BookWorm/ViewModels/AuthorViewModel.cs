using System.Collections.Generic;
using BookWorm.Models;

namespace BookWorm.ViewModels
{
    public class AuthorViewModel
    {
        public AuthorViewModel(Author author, List<Book> books)
        {
            Author = author;
            Books = books;
        }

        public List<Book> Books {get; set; }

        public Author Author { get; set; }
    }
}