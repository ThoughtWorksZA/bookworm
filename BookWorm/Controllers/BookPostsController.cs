using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookWorm.Models;

namespace BookWorm.Controllers
{
    public class BookPostsController : BaseController
    {
        public BookPostsController()
        {
        }

        public BookPostsController(Repository repository) : base(repository)
        {
        }

        public ViewResult List()
        {
            return View(_repository.List<BookPost>());
        }

        public ActionResult Create()
        {
            ViewBag.Title = "Add a Book Post";
            return View(new BookPost());
        }

        public ActionResult Edit(int id)
        {
            throw new NotImplementedException();
        }

        public ActionResult Details(int id)
        {
            throw new NotImplementedException();
        }

        public ActionResult Delete(int id)
        {
            throw new NotImplementedException();
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
    }
}
