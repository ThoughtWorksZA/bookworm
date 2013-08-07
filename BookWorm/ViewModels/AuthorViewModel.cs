using System.Collections.Generic;
using BookWorm.Models;

namespace BookWorm.ViewModels
{
    public class AuthorViewModel
    {
        public AuthorViewModel(Author author, List<Book> books, bool hasMoreBooks)
        {
            Author = author;
            Books = books;
            HasMoreBooks = hasMoreBooks;
        }

        public List<Book> Books {get; set; }

        public Author Author { get; set; }

        public bool HasMoreBooks { get; set; }
    }
}