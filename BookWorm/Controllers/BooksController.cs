using System.Web.Mvc;
using BookWorm.Models;

namespace BookWorm.Controllers
{
    public class BooksController : BaseController
    {
        public BooksController()
        {
        }

        public BooksController(Repository repository) : base (repository)
        {
        }

        public ViewResult Details(int id)
        {
            var book = (Book) _repository.Get<Book>(id);
            return View(book);
        }

        public ViewResult Create()
        {
            ViewBag.Title = "Add a Book";
            return View(new Book());
        }

        [HttpPost]
        public RedirectToRouteResult Create(Book book)
        {
            var createdBook = (Book) _repository.Create(book);
            TempData["flash"] = string.Format("Added {0} successfully", createdBook.Title);
            return RedirectToAction("Details", new { id = createdBook.Id });
        }

        public ViewResult List()
        {
            var books = _repository.List<Book>();
            return View(books);
        }
    }
}