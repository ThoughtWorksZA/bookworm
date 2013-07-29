using System.Collections.Generic;
using System.Linq;
using BookWorm.Models;
using BookWorm.Models.indices;
using Raven.Client;

namespace BookWorm.Services.FullTextSearch
{
    public class FullTextSearchService : IFullTextSearch
    {
        private readonly IDocumentSession _session;

        public FullTextSearchService(IDocumentSession session)
        {
            _session = session;
        }

        public List<Book> Search(string text)
        {
            var query = _session.Query<Book, Book_AllProperties>();

            var books = query.Search(b => b.Title, text)
                             .Search(b => b.Description, text)
                             .Search(b => b.Isbn, text)
                             .Search(b => b.Author, text)
                             .Search(b => b.Description, text)
                             .Search(b => b.Posts, text);
            return books.ToList();
        }
    }
}