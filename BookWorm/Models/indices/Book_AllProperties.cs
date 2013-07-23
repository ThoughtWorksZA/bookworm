using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;

namespace BookWorm.Models.indices
{
    public class Book_AllProperties : AbstractIndexCreationTask<Book>
    {
        public Book_AllProperties()
        {
            Map = books => from book in books
                           select new
                               {
                                   Title = book.Title,
                                   Subtitle = book.Subtitle,
                                   Author = book.Author,
                                   Isbn = book.Isbn,
                                   Description = book.Description
                               };

            Indexes = new Dictionary<Expression<Func<Book, object>>, FieldIndexing>
                {
                    {b=>b.Title, FieldIndexing.Analyzed},
                    {b=>b.Subtitle, FieldIndexing.Analyzed},
                    {b=>b.Author, FieldIndexing.Analyzed},
                    {b=>b.Isbn, FieldIndexing.Analyzed},
                    {b=>b.Description, FieldIndexing.Analyzed},
                }
            ;
        }
    }
}