using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using BookWorm.Models;
using Raven.Client;

namespace BookWorm.Controllers
{
    public abstract class BaseController : Controller
    {
        public IDocumentSession _session;
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
            var staticPages = _repository.List<StaticPage>();
            staticPages.Sort((x, y) => x.CreatedAt.CompareTo(y.CreatedAt));
            ViewBag.StaticPages = staticPages;
            foreach (StaticPage page in ViewBag.StaticPages)
            {
                _repository.Detach(page);
            }
            base.OnActionExecuting(filterContext);
        }

        protected virtual Repository GetRepository()
        {
            _session = GetDocumentStore().OpenSession();
            return new Repository(_session);
        }

        protected virtual IDocumentStore GetDocumentStore()
        {
            return ((MvcApplication)HttpContext.ApplicationInstance).Store;
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
