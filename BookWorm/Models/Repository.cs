using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Raven.Client;

namespace BookWorm.Models
{
    public class Repository
    {
        private IDocumentSession _documentSession;

        public Repository()
        {
        }

        public Repository(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }

        public void Dispose()
        {
            _documentSession.Dispose();
        }

        public virtual void UseOptimisticConcurrency()
        {
            _documentSession.Advanced.UseOptimisticConcurrency = true;
        }

        public virtual void Detach<T>(T model) where T : Model
        {
            _documentSession.Advanced.Evict(model);
        }

        public virtual T Create<T>(T model) where T : Model
        {
            model.CreatedAt = model.UpdatedAt = DateTime.Now;
            _documentSession.Store(model);
            return model;
        }

        public virtual void Edit<T>(T editedModel) where T : Model
        {
            editedModel.UpdatedAt = DateTime.Now;
            _documentSession.Store(editedModel);
        }

        public virtual void Delete<T>(int id) where T : Model
        {
            var model = Get<T>(id);
            _documentSession.Delete(model);
        }

        public virtual T Get<T>(int id) where T : Model
        {
            return _documentSession.Load<T>(id);
        }

        public virtual ICollection<T> List<T>() where T : Model
        {
            return List<T>(int.MaxValue);
        }

        public virtual ICollection<T> List<T>(int perPage) where T : Model
        {
            return List<T>(1, perPage);
        }

        public virtual ICollection<T> List<T>(int page, int perPage) where T : Model
        {
            var _ravenQueryable = _documentSession.Query<T>().OrderByDescending(d => d.UpdatedAt).Skip((page - 1) * perPage).Take(perPage);
            return _ravenQueryable.ToList();
        }

        public virtual ICollection<T> Search<T>(Expression<Func<T, bool>> predicate) where T : Model
        {
            return Search(predicate, 1, int.MaxValue);
        }

        public virtual ICollection<T> Search<T>(Expression<Func<T, bool>> predicate, int page, int perPage) where T : Model
        {
            var ravenQueryable = _documentSession.Query<T>();
            return ravenQueryable.Where(predicate).OrderByDescending(x => x.UpdatedAt).Skip((page - 1) * perPage).Take(perPage).ToList();
        }

        public virtual ICollection<T> Search<T>(Expression<Func<T, bool>> predicate, int perPage) where T : Model
        {
            return Search<T>(predicate, 1, perPage);
        }

        public virtual int Count<T>() where T : Model
        {
            return _documentSession.Query<T>().Count();
        }

        public virtual int Count<T>(Expression<Func<T, bool>> predicate) where T : Model
        {
            return _documentSession.Query<T>().Where(predicate).Count();
        }
    }

    public class Model
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
