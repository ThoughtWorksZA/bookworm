using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookWorm.Models;
using BookWorm.ViewModels;

namespace BookWorm.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController()
        {
        }

        public HomeController(Repository repository) : base(repository)
        {
        }

        public ActionResult Index()
        {
            var books = _repository.List<Book>(4);
            ViewBag.bookInformations = books.Select(book => new BookInformation(book)).ToList();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
