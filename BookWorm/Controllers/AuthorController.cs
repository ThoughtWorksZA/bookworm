using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BirdBrain;
using BookWorm.Models;
using BookWorm.ViewModels;
using Raven.Client;

namespace BookWorm.Controllers
{
    public class AuthorController : BaseController
    {

        public AuthorController()
        {

        }

        public AuthorController(Repository repository) : base(repository)
        {
            
        }

        public AuthorController(IDocumentSession session)
        {
            _session = session;
        }

        [HttpGet]
        public ViewResult List()
        {
            var authors = _session.Query<Author>().ToList();
            return View(authors);
        }

        [HttpGet]
        [Authorize(Roles = Roles.Admin)]
        public ViewResult Create()
        {
            return View(new Author());
        }

        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        public ActionResult Create(Author author)
        {
            if (ModelState.IsValid)
            {
                var existingAuthor = _session.Query<Author>().Customize(a => a.WaitForNonStaleResultsAsOfLastWrite()).FirstOrDefault(a=> a.Name == author.Name);

                if (existingAuthor != null)
                {
                    TempData["flashError"] = "An author with this name already exists";
                    return View(author);
                }

                _repository.Create(author);
                return RedirectToAction("List", "Author");
            }
            return null;
        }


    }
}
