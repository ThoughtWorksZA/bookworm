using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;
using Raven.Client.Linq.Indexing;

namespace BookWorm.Models.indices
{
    public class Book_AllProperties : AbstractIndexCreationTask<Book>
    {
        public Book_AllProperties()
        {
            Map = books => from book in books
                           select new
                               {
                                   Title = book.Title.Boost(10),
                                   Subtitle = book.Subtitle.Boost(8),
                                   Author = book.Author.Boost(8),
                                   Isbn = book.Isbn.Boost(10),
                                   Description = book.Description.Boost(5),
                                   Posts = book.Posts.Boost(1)
                               };

            Indexes = new Dictionary<Expression<Func<Book, object>>, FieldIndexing>
                {
                    {b=>b.Title, FieldIndexing.Analyzed},
                    {b=>b.Subtitle, FieldIndexing.Analyzed},
                    {b=>b.Author, FieldIndexing.Analyzed},
                    {b=>b.Isbn, FieldIndexing.Analyzed},
                    {b=>b.Description, FieldIndexing.Analyzed},
                    {b=>b.Posts, FieldIndexing.Analyzed},
                };
        }
    }
}