using System.Collections.Generic;
using System.Linq;
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
            _documentSession.Store(model);
            return model;
        }

        public void Dispose()
        {
            _documentSession.Dispose();
        }

        public virtual T Get<T>(int id) where T: Model<T>
        {
            return _documentSession.Load<T>(id);
        }

        public virtual ICollection<T>  List<T>() where T: Model<T>
        {
            return _documentSession.Query<T>().ToList();
        }
    }

    public class Model<T>
    {
        public int Id { get; set; }
    }
}
