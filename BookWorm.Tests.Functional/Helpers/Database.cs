using System.Security.Cryptography;
using System.Text;
using BirdBrain;
using BookWorm.Models;
using Raven.Client.Document;
using TechTalk.SpecFlow;

namespace BookWorm.Tests.Functional.Helpers
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
            DeleteAll<User>(documentStore);
        }

        private static void DeleteAll<T>(DocumentStore documentStore)
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

        private static string HashPassword(string password)
        {
            var encoder = new UTF8Encoding();
            var hashedPassword = encoder.GetString(SHA1.Create().ComputeHash(encoder.GetBytes(password)));
            return hashedPassword;
        }

        public static void CreateAdminUser()
        {
            var documentStore = FeatureContext.Current.Get<DocumentStore>();
            using (var session = documentStore.OpenSession())
            {
                session.Store(new User
                {
                    Username = Users.AdminUserName,
                    Password = HashPassword("password"),
                    IsApproved = true,
                    Roles = new []{"admin"},
                });
                session.SaveChanges();
            }
        }
    }
}
