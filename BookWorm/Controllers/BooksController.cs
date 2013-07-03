using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using BookWorm.Models;
using BookWorm.ViewModels;
using PagedList;
using Raven.Client.Linq;

namespace BookWorm.Controllers
{
    [Authorize]
    public class BooksController : BaseController
    {
        private static readonly object synclock = new object();
        private const string NoBooksFoundTxtSearch = "No books found that match your search. Change the search text to widen your search.";
        private const string NoBooksFoundTxtFilter = "No books found that match your search. Change the filter options on the left to widen your search.";

        public BooksController()
        {
        }

        public BooksController(Repository repository) : base (repository)
        {
        }

        [AllowAnonymous]
        public ViewResult List(int page = 1, int perPage = 9)
        {
            var books = _repository.List<Book>(page, perPage);
            ViewBag.Title = "Books";
            var bookInformations = new StaticPagedList<BookInformation>(books.Select(book => new BookInformation(book)).ToList(), page, perPage, _repository.Count<Book>());
            return View(new FilterInformation(bookInformations));
        }

        [AllowAnonymous]
        public ViewResult Details(int id)
        {
            var book = (Book) _repository.Get<Book>(id);
            var bookInformation = new BookInformation(book, book.Posts.Select(post => new BookPostInformation(book.Id, post)).ToList());
            ViewBag.Title = bookInformation.Model.Title;
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
              Book createdBook;

            //Warning: this wouldn't work in a cluster - need to use concurrency on the DB for atomicity
            lock (synclock) //Provide some atomicity for checking the ISBN exists and then create it
            {
                IEnumerable<Book> search = SearchByIsbn(bookInformation);
                if ( search.Any())
                {
                    TempData["flashError"] = "The ISBN number already exists";
                    return View(bookInformation);
                }
                createdBook = (Book) _repository.Create(bookInformation.Model);
            }
          
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
            lock (synclock)
            {
                IEnumerable<Book> search = SearchByIsbn(editedBookInformation);
                if (search.Any())
                {
                    TempData["flashError"] = "The Book Edit was not saved because the provided ISBN number already exists";
                    return View(editedBookInformation);
                }

                _repository.Edit(editedBookInformation.Model);
                
            }
            
            TempData["flashSuccess"] = string.Format("Updated {0} successfully", editedBookInformation.Model.Title);
            return RedirectToAction("Details", new { id = editedBookInformation.Model.Id });

        }

        private IEnumerable<Book> SearchByIsbn(BookInformation editedBookInformation)
        {
            return _repository.Search<Book>(b => b.Isbn == editedBookInformation.Model.Isbn)
                              .Where(b => b.Id != editedBookInformation.Model.Id)
                              .ToList();
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
        public ActionResult List(string searchQuery, int page = 1, int perPage = 9)
        {
            Expression<Func<Book, bool>> searchPredicate = book => book.Title == searchQuery || book.Isbn == searchQuery;
            var books = _repository.Search(searchPredicate, page, perPage);
            ViewBag.Title = string.Format("Search Results for \"{0}\"", searchQuery);
            var bookInformations = new StaticPagedList<BookInformation>(books.Select(book => new BookInformation(book)).ToList(), page, perPage, _repository.Count(searchPredicate));
            if (bookInformations.Count() == 1)
            {
                return RedirectToAction("Details", new { id = bookInformations.First().Model.Id });                
            }
            if (!bookInformations.Any())
            {
                TempData["flashNotice"] = NoBooksFoundTxtSearch;
            }
            ViewBag.HideFilter = true;
            return View(new FilterInformation(bookInformations));
        }

        [AllowAnonymous]
        public ActionResult Language(string languages, int page = 1, int perPage = 9)
        {
            Expression<Func<Book, bool>> searchPredicate = book => book.Language == languages;
            var bookInformations = DiscoverBooks(languages, page, perPage, searchPredicate);
            return View("List", new FilterInformation(new List<string>() {languages}, new List<string>(), new List<string>(), bookInformations));
        }

        private StaticPagedList<BookInformation> DiscoverBooks(string filterType, int page, int perPage, Expression<Func<Book, bool>> searchPredicate)
        {
            ViewBag.Title = string.Format("{0} Books", filterType);
            var books = _repository.Search(searchPredicate, page, perPage);
            var totalItemCount = _repository.Count(searchPredicate);
            var bookInformations = new StaticPagedList<BookInformation>(
                books.Select(book => new BookInformation(book)), page, perPage, totalItemCount);
            return bookInformations;
        }

        [AllowAnonymous]
        public ActionResult Filter(List<string> languages, List<string> ageRanges, List<string> genres, int page = 1, int perPage = 9)
        {
            languages = languages ?? new List<string>();
            ageRanges = ageRanges ?? new List<string>();
            genres = genres ?? new List<string>();
            var books = _repository.List<Book>();
            if (languages.Any())
            {
                books = books.Where(book => book.Language.In(languages)).ToList();
            }
            if (ageRanges.Any())
            {
                books = books.Where(book => book.AgeRange.In(ageRanges)).ToList();
            }
            if (genres.Any())
            {
                books = books.Where(book => book.Genre.In(genres)).ToList();
            }
            ViewBag.Title = "Books";
            var bookInformations = books.Select(book => new BookInformation(book)).ToList().ToPagedList(page, perPage);

            if (!bookInformations.Any())
            {
                TempData["flashNotice"] = NoBooksFoundTxtFilter;
            }
            return View("List", new FilterInformation(languages, ageRanges, genres, bookInformations)); 
        }

        [AllowAnonymous]
        public ActionResult AgeRange(string ageRanges, int page = 1, int perPage = 9)
        {
            Expression<Func<Book, bool>> searchPredicate = book => book.AgeRange == ageRanges;
            var bookInformations = DiscoverBooks(ageRanges, page, perPage, searchPredicate);
            return View("List", new FilterInformation(new List<string>(), new List<string>() { ageRanges }, new List<string>(), bookInformations));
        }

        [AllowAnonymous]
        public ActionResult Genre(string genres, int page = 1, int perPage = 9)
        {
            Expression<Func<Book, bool>> searchPredicate = book => book.Genre == genres;
            var bookInformations = DiscoverBooks(genres, page, perPage, searchPredicate);
            return View("List", new FilterInformation(new List<string>(), new List<string>(), new List<string>() { genres }, bookInformations));
        }
    }
}