using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookWorm.Models;
using BookWorm.ViewModels;
using MarkdownSharp;

namespace BookWorm.Controllers
{
    [Authorize]
    public class BookPostsController : BaseController
    {
        public BookPostsController()
        {
        }

        public BookPostsController(Repository repository) : base(repository)
        {
        }

        [AllowAnonymous]
        public ViewResult List()
        {
            return View(_repository.List<BookPost>());
        }

        [AllowAnonymous]
        public ViewResult Details(int id, int bookId)
        {
            var book = _repository.Get<Book>(bookId);
            var bookPost = book.Posts.First(post => post.Id == id);
            ViewBag.transformedContent = new Markdown().Transform(bookPost.Content);
            return View(new BookPostInformation(bookId, bookPost, book));
        }

        public ActionResult Create(int bookId)
        {
            ViewBag.Title = "Add a Book Post";
            return View(new BookPostInformation(bookId, new BookPost{CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now}));
        }

        [HttpPost]
        public ActionResult Create(BookPostInformation submittedBookPostInformation)
        {
            if (!ModelState.IsValid)
            {
                TempData["flashError"] = "There were problems saving this book post";
                return View(submittedBookPostInformation);
            }
            _repository.UseOptimisticConcurrency();
            var book = _repository.Get<Book>(submittedBookPostInformation.BookId);
            var bookPost = submittedBookPostInformation.Model;
            if (book.Posts.Any())
            {
                bookPost.Id = book.Posts.Max(post => post.Id) + 1;
            }
            else
            {
                bookPost.Id = 1;
            }
            book.Posts.Add(bookPost);
            _repository.Edit(book);
            TempData["flashSuccess"] = string.Format("Added {0} successfully", bookPost.Title);
            return RedirectToAction("Details", "Books", new { id = book.Id });
        }

        public ViewResult Edit(int id, int bookId)
        {
            ViewBag.Method = "PUT";
            var bookPost = _repository.Get<Book>(bookId).Posts.First(post => post.Id == id);
            return View(new BookPostInformation(bookId, bookPost));
        }

        [HttpPut]
        public ActionResult Edit(BookPostInformation editedBookPostInformation)
        {
            if (!ModelState.IsValid)
            {
                TempData["flashError"] = "There were problems saving this book post";
                return View(editedBookPostInformation);
            }
            var book = _repository.Get<Book>(editedBookPostInformation.BookId);
            var editedBookPost = editedBookPostInformation.Model;
            editedBookPost.UpdatedAt = DateTime.Now;
            var oldBookPost = book.Posts.First(post => post.Id == editedBookPost.Id);
            book.Posts.Remove(oldBookPost);
            book.Posts.Add(editedBookPost);
            _repository.Edit(book);
            TempData["flashSuccess"] = string.Format("Updated {0} successfully", editedBookPost.Title);
            return RedirectToAction("Details", new { id = editedBookPost.Id, bookId = book.Id });
        }

        [HttpDelete]
        public RedirectToRouteResult Delete(int id, int bookId)
        {
            var book = _repository.Get<Book>(bookId);
            var bookPost = book.Posts.First(post => post.Id == id);
            book.Posts.Remove(bookPost);
            _repository.Edit(book);
            TempData["flashSuccess"] = string.Format("Book Post successfully deleted");
            return RedirectToAction("Details", "Books", new {id = book.Id});
        }

    }
}
