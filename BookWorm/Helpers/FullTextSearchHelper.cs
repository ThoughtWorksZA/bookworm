using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BookWorm.Models;
using BookWorm.Models.indices;
using Raven.Client;

namespace BookWorm.Helpers
{
    public class FullTextSearchHelper
    {
        private readonly IDocumentSession _session;

        public FullTextSearchHelper(IDocumentSession session)
        {
            _session = session;
        }

        public List<Book> SearchByFullText(string text)
        {
            var terms = text.Split(' ');
            var query = _session.Query<Book, Book_AllProperties>();


            Expression<Func<Book, object>> fieldSelector = x => x.Description;
            var ravenQueryable = terms.Aggregate(query, (q, term) => q.Search(fieldSelector, term, options: SearchOptions.And));
            var byDescription = ravenQueryable.ToList();
            return byDescription;
        }
    }
}