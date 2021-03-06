﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using BookWorm.Models;
using BookWorm.Services.FullTextSearch;
using BookWorm.ViewModels;
using PagedList;
using Raven.Client.Linq;

namespace BookWorm.Controllers
{
    [Authorize]
    public class BooksController : BaseController
    {
        private const string NoBooksFoundTxtSearch = "No books found that match your search. Change the search text to widen your search.";
        private const string NoBooksFoundTxtFilter = "No books found that match your search. Change the filter options on the left to widen your search.";
        private const string ProblemsSavingBookMessage = "There were problems saving this book";

        private IFullTextSearch _fullTextSearch;
        
        public BooksController()
        {
        }

        public BooksController(IFullTextSearch fullTextSearch)
        {
            _fullTextSearch = fullTextSearch;
        }

        public BooksController(Repository repository) : base (repository)
        {
        }

        [AllowAnonymous]
        [HttpGet]
        public ViewResult List(int page = 1, int perPage = 9)
        {
            var books = Repository.List<Book>(page, perPage);
            ViewBag.Title = "Books";
            var bookInformations = new StaticPagedList<BookInformation>(books.Select(book => new BookInformation(book)).ToList(), page, perPage, Repository.Count<Book>());
            return View(new FilterInformation(bookInformations));
        }

        [AllowAnonymous]
        public ViewResult Details(int id)
        {
            var book = Repository.Get<Book>(id);

            if (book == null)
            {
                throw new HttpException(404, "The requested book could not be found");
            }

            var bookPosts = book.Posts.Select(post => new BookPostInformation(book.Id, post)).ToList();
            var bookInformation = new BookInformation(book, bookPosts);
            ViewBag.Title = bookInformation.Model.Title;
            ViewBag.MetaDescription = bookInformation.Summary(155);
            return View(bookInformation);
        }

        [Authorize(Roles = Roles.Admin +"," + Roles.Author)]
        public ViewResult Create()
        {
            ViewBag.Title = "Add a Book";
            return View(new BookInformation(new Book()));
        }

        [HttpPost]
        [Authorize(Roles = Roles.Admin + "," + Roles.Author)]
        public ActionResult Create(BookInformation bookInformation)
        {
            if (!ModelState.IsValid)
            {
                TempData["flashError"] = ProblemsSavingBookMessage;
                return View(bookInformation);
            }

            if (!IsIsbnUnique(bookInformation))
            {
                TempData["flashError"] = "The ISBN number already exists";
                return View(bookInformation);
            }
            var createdBook = Repository.Create(bookInformation.Model);
          
            TempData["flashSuccess"] = string.Format("Added {0} successfully", createdBook.Title);
            return RedirectToAction("Details", new { id = createdBook.Id });
        }

        [Authorize(Roles = Roles.Admin + "," + Roles.Author)]
        public ViewResult Edit(int id)
        {
            ViewBag.Title = "Edit a Book";
            ViewBag.Method = "PUT";
            return View(new BookInformation(Repository.Get<Book>(id)));
        }

        [HttpPut]
        [Authorize(Roles = Roles.Admin + "," + Roles.Author)]
        public ActionResult Edit(BookInformation editedBookInformation)
        {
            if (!ModelState.IsValid)
            {
                TempData["flashError"] = ProblemsSavingBookMessage;
                return View(editedBookInformation);
            }

            if (!IsIsbnUnique(editedBookInformation))
            {
                TempData["flashError"] = "The Book Edit was not saved because the provided ISBN number already exists";
                return View(editedBookInformation);
            }

            Repository.Edit(editedBookInformation.Model);
            
            TempData["flashSuccess"] = string.Format("Updated {0} successfully", editedBookInformation.Model.Title);
            return RedirectToAction("Details", new { id = editedBookInformation.Model.Id });
        }

        private bool IsIsbnUnique(BookInformation bookInformation)
        {
            return bookInformation.Model.Isbn == null || !SearchByIsbn(bookInformation).Any();
        }

        private IEnumerable<Book> SearchByIsbn(BookInformation editedBookInformation)
        {
            return Repository.Search<Book>(b => (b.Isbn == editedBookInformation.Model.Isbn && b.Id != editedBookInformation.Model.Id))
                              
                              .ToList();
        }

        [HttpDelete]
        [Authorize(Roles = Roles.Admin + "," + Roles.Author)]
        public RedirectToRouteResult Delete(int id)
        {
            Repository.Delete<Book>(id);
            TempData["flashSuccess"] = string.Format("Book successfully deleted");
            return RedirectToAction("List");
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Search(string searchQuery, int page = 1, int perPage = 9)
        {
            if (_fullTextSearch == null)
            {
                _fullTextSearch = new FullTextSearchService(DocumentSession);
            }
            var books = _fullTextSearch.Search(searchQuery);
            ViewBag.Title = string.Format("Search Results for \"{0}\"", searchQuery);
            var bookInformations = new StaticPagedList<BookInformation>(books.Skip((page-1)*perPage).Take(perPage)
                .Select(book => new BookInformation(book))
                .ToList(), page, perPage, books.Count);

            if (books.Count() == 1)
            {
                return RedirectToAction("Details", new { id = bookInformations.First().Model.Id });                
            }
            if (!bookInformations.Any())
            {
                TempData["flashNotice"] = NoBooksFoundTxtSearch;
            }
            ViewBag.HideFilter = true;
            ViewBag.SearchQuery = searchQuery;
            return View("List", new FilterInformation(bookInformations));
        }

        [AllowAnonymous]
        public ActionResult Language(string languages, int page = 1, int perPage = 9)
        {
            Expression<Func<Book, bool>> searchPredicate = book => book.Language == languages;
            var bookInformations = DiscoverBooks(languages, page, perPage, searchPredicate);
            return View("List", new FilterInformation(new List<string> {languages}, new List<string>(), new List<string>(), bookInformations));
        }

        private StaticPagedList<BookInformation> DiscoverBooks(string filterType, int page, int perPage, Expression<Func<Book, bool>> searchPredicate)
        {
            ViewBag.Title = string.Format("{0} Books", filterType);
            var books = Repository.Search(searchPredicate, page, perPage);
            var totalItemCount = Repository.Count(searchPredicate);
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
            var books = Repository.Query<Book>();

            if (ageRanges.Any())
            {
                books = books.Where(book => book.AgeRange.In(ageRanges));
            }
            if (languages.Any())
            {
                books = books.Where(book => book.Language.In(languages));
            }
            if (genres.Any())
            {
                books = books.Where(book => book.Genre.In(genres));
            }

            ViewBag.Title = "Books";

            var bookPage = books.OrderByDescending(b => b.UpdatedAt)
                .Skip((page - 1)*perPage).Take(perPage)
                .ToList();
            
            var bookInformations = new StaticPagedList<BookInformation>(
                bookPage.Select(book => new BookInformation(book)), page, perPage, books.Count());

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
            return View("List", new FilterInformation(new List<string>(), new List<string> { ageRanges }, new List<string>(), bookInformations));
        }

        [AllowAnonymous]
        public ActionResult Genre(string genres, int page = 1, int perPage = 9)
        {
            Expression<Func<Book, bool>> searchPredicate = book => book.Genre == genres;
            var bookInformations = DiscoverBooks(genres, page, perPage, searchPredicate);
            return View("List", new FilterInformation(new List<string>(), new List<string>(), new List<string> { genres }, bookInformations));
        }
    }
}
