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

        public ViewResult List()
        {
            var books = _repository.List<Book>();
            return View(books);
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
        public ActionResult Create(Book book)
        {

            if (! ModelState.IsValid )
            {
                TempData["flash"] = "There were problems saving this book";
                return View(book);
            }

            var createdBook = (Book) _repository.Create(book);
            TempData["flash"] = string.Format("Added {0} successfully", createdBook.Title);
            return RedirectToAction("Details", new { id = createdBook.Id });
        }

        public ViewResult Edit(int id)
        {
            ViewBag.Title = "Edit a Book";
            return View(_repository.Get<Book>(id));
        }

        [HttpPut]
        public ActionResult Edit(Book editedBook)
        {
            if (!ModelState.IsValid)
            {
                TempData["flash"] = "There were problems saving this book";
                return View(editedBook);
            }

            _repository.Edit(editedBook);
            TempData["flash"] = string.Format("Updated {0} successfully", editedBook.Title);
            return RedirectToAction("Details", new { id = editedBook.Id });

        }

        [HttpDelete]
        public RedirectToRouteResult Delete(int id)
        {
            _repository.Delete<Book>(id);
            TempData["flash"] = string.Format("Book successfully deleted");            
            return RedirectToAction("List");
        }
    }
}