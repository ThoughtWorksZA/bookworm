using BookWorm.Models;
using Raven.Client;
using Raven.Client.Document;

namespace BookWorm.Repository
{
    public class Repository<Model>
    {
        private readonly IDocumentSession _documentSession;

        public Repository(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }

        public virtual Model<Model> Create(Model model)
        {
            throw new System.NotImplementedException();
        }

        public void Dispose()
        {
            _documentSession.Dispose();
        }
    }

    public class Model<T>
    {
        public int Id { get; set; }
    }
}