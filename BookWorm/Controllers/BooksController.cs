using System;
using System.Web.Mvc;
using BookWorm.Models;

namespace BookWorm.Controllers
{
    public class BooksController : Controller
    {
        [HttpPost]
        public ViewResult New(Book book)
        {
            ViewBag.Title = "Success";
            ViewBag.SuccessMessage = String.Format("Created book '{0}'", book.Title);
            return View();
        }

        public ViewResult New()
        {
            ViewBag.Title = "Add a Book";
            return View(new Book());
        }
    }
}