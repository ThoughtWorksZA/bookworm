using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BookWorm.Models;

namespace BookWorm.ViewModels
{
    public class BookInformation
    {
        public Book Book { get; set; }

        public BookInformation(Book book)
        {
            this.Book = book;
        }
        public BookInformation()
        {
        }
    }
}