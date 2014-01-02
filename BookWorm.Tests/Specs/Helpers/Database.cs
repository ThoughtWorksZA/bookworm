using BookWorm.Models;
using Raven.Client.Document;
using TechTalk.SpecFlow;

namespace BookWorm.Tests.Specs.Helpers
{
    public static class Database
    {
        public static void SetupDocumentStore()
        {
            var documentStore = new DocumentStore
            {
                ConnectionStringName = "RavenDB"
            };
            documentStore.Initialize();
            
            FeatureContext.Current.Set(documentStore);
        }

        public static void ClearDatabase()
        {
            var documentStore = FeatureContext.Current.Get<DocumentStore>();

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
