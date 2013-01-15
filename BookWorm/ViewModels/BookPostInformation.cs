using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
    }
}