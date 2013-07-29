using System.Collections.Generic;
using BookWorm.Models;

namespace BookWorm.Services.FullTextSearch
{
    public interface IFullTextSearch
    {
        List<Book> Search(string text);
    }
}