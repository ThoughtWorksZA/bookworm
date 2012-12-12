using System.Collections.Generic;
using System.Web.Mvc;
using BookWorm.Models;
using Raven.Client;

namespace BookWorm.Controllers
{
    public abstract class BaseController : Controller
    {
        protected IDocumentSession _session;
        protected Repository _repository;

        public BaseController()
        {
        }

        public BaseController(Repository repository)
        {
            _repository = repository;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _repository = GetRepository();
            ViewBag.StaticPages = _repository.List<StaticPage>();
            base.OnActionExecuting(filterContext);
        }

        protected virtual Repository GetRepository()
        {
            _session = GetDocumentStore().OpenSession();
            return new Repository(_session);
        }

        protected virtual IDocumentStore GetDocumentStore()
        {
            return MvcApplication.Store;
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.IsChildAction)
                return;

            using (_session)
            {
                if (filterContext.Exception != null)
                    return;

                if (_session != null)
                    _session.SaveChanges();
            }
        }
    }
}
