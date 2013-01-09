using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        public virtual T Create<T>(T model)
        {
            _documentSession.Store(model);
            return model;
        }

        public void Dispose()
        {
            _documentSession.Dispose();
        }

        public virtual T Get<T>(int id)
        {
            return _documentSession.Load<T>(id);
        }

        public virtual ICollection<T> List<T>()
        {
            var _ravenQueryable = _documentSession.Query<T>();
            return _ravenQueryable.ToList();
        }

        public virtual void Delete<T>(int id)
        {
            var model = Get<T>(id);
            _documentSession.Delete(model);
        }

        public virtual void Edit<T>(T editedModel)
        {
            _documentSession.Store(editedModel);
        }

        public virtual ICollection<T> Search<T>(Expression<Func<T, bool>> predicate)
        {
            var ravenQueryable = _documentSession.Query<T>();
            return ravenQueryable.Where(predicate).ToList();
        }

        public virtual void Detach<T>(T model)
        {
            _documentSession.Advanced.Evict(model);
        }

        public virtual bool Any<T>()
        {
            return _documentSession.Query<T>().Any();
        }
    }

    public class Model
    {
        public int Id { get; set; }
    }
}
