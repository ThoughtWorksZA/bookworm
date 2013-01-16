using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BookWorm.Models;
using BookWorm.Models.Validations;
using BookWorm.ViewModels;
using Raven.Client.Linq;

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
            ViewBag.Title = "Books";
            var bookInformations = books.Select(book => new BookInformation(book)).ToList();
            return View(new FilterInformation(ValidLanguage.ValidLanguages, bookInformations));
        }

        [AllowAnonymous]
        public ViewResult Details(int id)
        {
            var book = (Book) _repository.Get<Book>(id);
            var bookInformation = new BookInformation(book, book.Posts.Select(post => new BookPostInformation(book.Id, post)).ToList());
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
                TempData["flashError"] = "There were problems saving this book";
                return View(bookInformation);
            }

            var createdBook = (Book) _repository.Create(bookInformation.Book);
            TempData["flashSuccess"] = string.Format("Added {0} successfully", createdBook.Title);
            return RedirectToAction("Details", new { id = createdBook.Id });
        }

        public ViewResult Edit(int id)
        {
            ViewBag.Title = "Edit a Book";
            ViewBag.Method = "PUT";
            return View(new BookInformation(_repository.Get<Book>(id)));
        }

        [HttpPut]
        public ActionResult Edit(BookInformation editedBookInformation)
        {
            if (!ModelState.IsValid)
            {
                TempData["flashError"] = "There were problems saving this book";
                return View(editedBookInformation);
            }

            _repository.Edit(editedBookInformation.Book);
            TempData["flashSuccess"] = string.Format("Updated {0} successfully", editedBookInformation.Book.Title);
            return RedirectToAction("Details", new { id = editedBookInformation.Book.Id });

        }

        [HttpDelete]
        public RedirectToRouteResult Delete(int id)
        {
            _repository.Delete<Book>(id);
            TempData["flashSuccess"] = string.Format("Book successfully deleted");            
            return RedirectToAction("List");
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult List(string searchQuery)
        {
            var books = _repository.Search<Book>(book => book.Title == searchQuery || book.Isbn == searchQuery);
            ViewBag.Title = string.Format("Search Results for \"{0}\"", searchQuery);
            var bookInformations = books.Select(book => new BookInformation(book)).ToList();
            if (bookInformations.Count() == 1)
            {
                return RedirectToAction("Details", new { id = bookInformations.First().Book.Id });                
            }
            if (!bookInformations.Any())
            {
                TempData["flashNotice"] = "No books found that match your search.";
            }
            ViewBag.HideFilter = true;
            return View(new FilterInformation(bookInformations));
        }

        [AllowAnonymous]
        public ActionResult Language(string language)
        {
            var books = _repository.Search<Book>(book => book.Language == language);
            ViewBag.Title = string.Format("{0} Books", language);
            var bookInformations = books.Select(book => new BookInformation(book)).ToList();
            if (bookInformations.Count() == 1)
            {
                return RedirectToAction("Details", new { id = bookInformations.First().Book.Id });
            }
            return View("List", new FilterInformation(new List<string>() {language}, bookInformations));
        }

        [AllowAnonymous]
        public ActionResult Filter(List<string> languages)
        {
            var books = _repository.Search<Book>(book => book.Language.In(languages));
            ViewBag.Title = string.Format("{0} Books", string.Join(",", languages));
            var bookInformations = books.Select(book => new BookInformation(book)).ToList();
            return View("List", new FilterInformation(languages, bookInformations)); 
        }
    }
}