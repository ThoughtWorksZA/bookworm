using Raven.Client;

namespace BookWorm.Models
{
    public class Repository
    {
        private readonly IDocumentSession _documentSession;

        public Repository()
        {
        }

        public Repository(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }

        public virtual Model<T> Create<T>(T model) where T : Model<T>
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