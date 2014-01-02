using BookWorm.Models;
using Raven.Client.Document;
using TechTalk.SpecFlow;

namespace BookWorm.Tests.Specs.Helpers
{
    [Binding]
    public static class Database
    {
        
        [BeforeScenario]
        public static void Setup()
        {
            var documentStore = new DocumentStore
            {
                ConnectionStringName = "RavenDB"
            };
            documentStore.Initialize();

            DeleteAll<Book>(documentStore);
            DeleteAll<StaticPage>(documentStore);
        }

        private static void DeleteAll<T>(DocumentStore documentStore) where T : Model
        {
            using (var session = documentStore.OpenSession())
            {
                var books = session.Query<T>();
                foreach (var book in books)
                {
                    session.Delete(book);
                }
                session.SaveChanges();
            }
        }
    }
}
