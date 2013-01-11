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

        public virtual T Create<T>(T model) where T : Model
        {
            model.CreatedAt = model.UpdatedAt = DateTime.Now;
            _documentSession.Store(model);
            return model;
        }

        public void Dispose()
        {
            _documentSession.Dispose();
        }

        public virtual T Get<T>(int id) where T : Model
        {
            return _documentSession.Load<T>(id);
        }

        public virtual ICollection<T> List<T>() where T : Model
        {
            return List<T>(int.MaxValue);
        }

        public virtual void Delete<T>(int id) where T : Model
        {
            var model = Get<T>(id);
            _documentSession.Delete(model);
        }

        public virtual void Edit<T>(T editedModel) where T : Model
        {
            editedModel.UpdatedAt = DateTime.Now;
            _documentSession.Store(editedModel);
        }

        public virtual ICollection<T> Search<T>(Expression<Func<T, bool>> predicate) where T : Model
        {
            var ravenQueryable = _documentSession.Query<T>();
            return ravenQueryable.Where(predicate).ToList();
        }

        public virtual void Detach<T>(T model) where T : Model
        {
            _documentSession.Advanced.Evict(model);
        }

        public ICollection<T> List<T>(int perPage) where T : Model
        {
            return List<T>(1, perPage);
        }

        public ICollection<T> List<T>(int page, int perPage) where T : Model
        {
            var _ravenQueryable = _documentSession.Query<T>().OrderBy(d => d.CreatedAt).Skip((page - 1) * perPage).Take(perPage);
            return _ravenQueryable.ToList();
        }
    }

    public class Model
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
