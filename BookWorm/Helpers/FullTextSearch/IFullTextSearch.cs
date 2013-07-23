using System.Collections.Generic;
using BookWorm.Models;

namespace BookWorm.Helpers.FullTextSearch
{
    public interface IFullTextSearch
    {
        List<Book> Search(string text);
    }
}