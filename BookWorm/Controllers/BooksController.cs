using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BookWorm.Models;
using BookWorm.ViewModels;

namespace BookWorm.Controllers
{
    [Authorize]
    public class BooksController : BaseController
    {
        public BooksController()
        {
        }

        public BooksController(Repository repository) : base (repository)
        {
        }

        [AllowAnonymous]
        public ViewResult List()
        {
            var books = _repository.List<Book>();
            var bookInformations = books.Select(book => new BookInformation(book)).ToList();
            return View(bookInformations);
        }

        [AllowAnonymous]
        public ViewResult Details(int id)
        {
            var book = (Book) _repository.Get<Book>(id);
            var bookInformation = new BookInformation(book);
            ViewBag.Title = bookInformation.Book.Title;
            return View(bookInformation);
        }

        public ViewResult Create()
        {
            ViewBag.Title = "Add a Book";
            return View(new BookInformation(new Book()));
        }

        [HttpPost]
        public ActionResult Create(BookInformation bookInformation)
        {

            if (! ModelState.IsValid )
            {
                TempData["flash"] = "There were problems saving this book";
                return View(bookInformation);
            }

            var createdBook = (Book) _repository.Create(bookInformation.Book);
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