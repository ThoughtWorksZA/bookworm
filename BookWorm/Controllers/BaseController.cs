using System.Web.Mvc;
using BookWorm.Models;
using Raven.Client;

namespace BookWorm.Controllers
{
    public abstract class BaseController : Controller
    {
        protected IDocumentSession DocumentSession;
        protected Repository Repository;

        public BaseController()
        {
        }

        public BaseController(Repository repository)
        {
            Repository = repository;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Repository = GetRepository();
            var staticPages = Repository.List<StaticPage>();
            staticPages.Sort((x, y) => x.CreatedAt.CompareTo(y.CreatedAt));
            ViewBag.StaticPages = staticPages;
            foreach (StaticPage page in ViewBag.StaticPages)
            {
                Repository.Detach(page);
            }
            base.OnActionExecuting(filterContext);
        }

        protected virtual Repository GetRepository()
        {
            DocumentSession = GetDocumentStore().OpenSession();
            return new Repository(DocumentSession);
        }

        protected virtual IDocumentStore GetDocumentStore()
        {
            return ((MvcApplication)HttpContext.ApplicationInstance).Store;
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.IsChildAction)
                return;

            using (DocumentSession)
            {
                if (filterContext.Exception != null)
                    return;

                if (DocumentSession != null)
                    DocumentSession.SaveChanges();
            }
        }
    }
}
