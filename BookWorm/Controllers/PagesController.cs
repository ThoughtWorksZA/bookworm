using System.Web.Mvc;
using BookWorm.Models;
using MarkdownSharp;
using Raven.Client.Exceptions;

namespace BookWorm.Controllers
{
    [Authorize]
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
                StaticPage savedPage = _repository.Create(submittedStaticPage);
                TempData["flashSuccess"] = string.Format("Added {0}", submittedStaticPage.Title);
                return RedirectToAction("Details", "Pages", new {id = savedPage.Id});
            }
            catch (NonUniqueObjectException ex)
            {
                TempData["flashError"] = string.Format("Sorry, page {0} already exists.", submittedStaticPage.Title);
                return View();
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public ViewResult Details(int id)
        {
            var page = _repository.Get<StaticPage>(id);
            ViewBag.transformedContent = new Markdown().Transform(page.Content);
            return View(page);
        }


        [HttpGet]
        [AllowAnonymous]
        public ViewResult List()
        {
            return View(_repository.List<StaticPage>());
        }

        [HttpDelete]
        public RedirectToRouteResult Delete(int id)
        {
            _repository.Delete<StaticPage>(id);
            TempData["flashSuccess"] = string.Format("Page successfully deleted"); 
            return RedirectToAction("List", "Pages");
        }

        [HttpGet]
        public ViewResult Edit(int id)
        {
            return View(_repository.Get<StaticPage>(id));
        }

        [HttpPut]
        public RedirectToRouteResult Edit(StaticPage updatedPage)
        {
            _repository.Edit(updatedPage);
            TempData["flashSuccess"] = string.Format("Updated {0} successfully", updatedPage.Title);
            return RedirectToAction("Details", new { id = updatedPage.Id });
        }
    }
}