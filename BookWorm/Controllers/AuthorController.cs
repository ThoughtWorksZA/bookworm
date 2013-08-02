using System;
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
                _repository.Create(author);
                return RedirectToAction("List", "Author");
            }
            return null;
        }


    }
}
