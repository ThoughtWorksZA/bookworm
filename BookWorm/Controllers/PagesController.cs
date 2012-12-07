using System;
using System.Web.Mvc;
using BookWorm.Models;

namespace BookWorm.Controllers
{
    public class PagesController : Controller
    {
        private readonly Repository _repository;

        public PagesController(Repository repository)
        {
            _repository = repository;
        }

        public ActionResult New(StaticPage submittedStaticPage)
        {
            try
            {
                var savedPage = _repository.Create(submittedStaticPage);
                TempData["flash"] = string.Format("Added {0}", submittedStaticPage.Title);
                return RedirectToAction("index", "pages", new { id = savedPage.Id }); 
            }
            catch (Raven.Client.Exceptions.NonUniqueObjectException ex)
            {
                TempData["flash"] = string.Format("Sorry, page {0} already exists.", submittedStaticPage.Title);
                return View();
            }
        }

        public ActionResult Index(int id)
        {
            throw new NotImplementedException();
        }
    }
}