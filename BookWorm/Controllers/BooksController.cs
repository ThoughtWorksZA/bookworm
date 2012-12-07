using System;
using System.Web.Mvc;
using BookWorm.Models;

namespace BookWorm.Controllers
{
    public class BooksController : Controller
    {
        private Repository _repository;

        public BooksController(Repository repository)
        {
            _repository = repository;
        }

        public BooksController()
        {

        }

        [HttpPost]
        public ViewResult New(Book book)
        {
            _repository.Create(book);
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