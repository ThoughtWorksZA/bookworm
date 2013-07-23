using System.Linq;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;

namespace BookWorm.Models.indices
{
    public class Book_AllProperties: AbstractIndexCreationTask<Book, Book_AllProperties.Result>
{
    public class Result
    {
        public string Query { get; set; }
    }
 
    public Book_AllProperties()
    {
        Map = books => from book in books
                       select new
                           {
                               Query = AsDocument(book).Select(x => x.Value)
                           };
 
        Index(x => x.Query, FieldIndexing.Analyzed);
    }
}
}