using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookWorm.Models;

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
        public ViewResult Details(int id)
        {
            var bookPost = _repository.Get<BookPost>(id);
            //            ViewBag.transformedContent = new Markdown().Transform(page.Content);
            return View(bookPost);
        }

        public ActionResult Create()
        {
            ViewBag.Title = "Add a Book Post";
            return View(new BookPost());
        }

        [HttpPost]
        public ActionResult Create(BookPost submittedBookPost)
        {
            if (!ModelState.IsValid)
            {
                TempData["flashError"] = "There were problems saving this book post";
                return View(submittedBookPost);
            }

            BookPost savedBookPost = _repository.Create(submittedBookPost);
            TempData["flashSuccess"] = string.Format("Added {0} successfully", submittedBookPost.Title);
            return RedirectToAction("Details", "BookPosts", new { id = savedBookPost.Id });
        }

        public ViewResult Edit(int id)
        {
            ViewBag.Method = "PUT";
            return View(_repository.Get<BookPost>(id));
        }

        [HttpPut]
        public ActionResult Edit(BookPost editedBookPost)
        {
            if (!ModelState.IsValid)
            {
                TempData["flashError"] = "There were problems saving this book post";
                return View(editedBookPost);
            }

            _repository.Edit(editedBookPost);
            TempData["flashSuccess"] = string.Format("Updated {0} successfully", editedBookPost.Title);
            return RedirectToAction("Details", new { id = editedBookPost.Id });

        }

        [HttpDelete]
        public RedirectToRouteResult Delete(int id)
        {
            _repository.Delete<BookPost>(id);
            TempData["flashSuccess"] = string.Format("Book Post successfully deleted");
            return RedirectToAction("List");
        }

    }
}
