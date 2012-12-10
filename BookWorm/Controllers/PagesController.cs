using System.Web.Mvc;
using BookWorm.Models;
using MarkdownSharp;

namespace BookWorm.Controllers
{
    public class PagesController : BaseController
    {
        public PagesController()
        {
        }

        public PagesController(Repository repository) : base(repository)
        {
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View("Create", new StaticPage());
        }

        [HttpPost]
        public ActionResult Create(StaticPage submittedStaticPage)
        {
            try
            {
                var savedPage = _repository.Create(submittedStaticPage);
                TempData["flash"] = string.Format("Added {0}", submittedStaticPage.Title);
                return RedirectToAction("Details", "Pages", new { id = savedPage.Id }); 
            }
            catch (Raven.Client.Exceptions.NonUniqueObjectException ex)
            {
                TempData["flash"] = string.Format("Sorry, page {0} already exists.", submittedStaticPage.Title);
                return View();
            }
        }

        [HttpGet]
        public ViewResult Details(int id)
        {
            var page = _repository.Get<StaticPage>(id);
            ViewBag.transformedContent = new Markdown().Transform(page.Content);
            return View(page);
        }


        [HttpGet]
        public ViewResult List()
        {
            return View(_repository.List<StaticPage>());
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Delete)]
        public RedirectToRouteResult Delete(int id)
        {
            _repository.Delete<StaticPage>(id);
            return RedirectToAction("List", "Pages");
        }
    }
}