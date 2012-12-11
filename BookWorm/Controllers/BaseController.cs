using System.Web.Mvc;
using BookWorm.Models;
using Raven.Client;

namespace BookWorm.Controllers
{
    public abstract class BaseController : Controller
    {
        private IDocumentSession _session;
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

            var allPages = _repository.List<StaticPage>();
            ViewBag.StaticPages = allPages;
            _session = GetDocumentStore().OpenSession();
            _repository = new Repository(_session);
            base.OnActionExecuting(filterContext);

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
