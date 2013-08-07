using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BirdBrain;
using BookWorm.Models;
using BookWorm.ViewModels;
using PagedList;
using Raven.Client;

namespace BookWorm.Controllers
{
    public class AuthorsController : BaseController
    {
        public AuthorsController()
        {
        }

        public AuthorsController(Repository repository) : base(repository)
        {
        }

        public AuthorsController(IDocumentSession session)
        {
            _session = session;
        }

        [HttpGet]
        [AllowAnonymous]
        public ViewResult List(int page = 1, int perPage = 9)
        {
            var authors = _repository.List<Author>(page, perPage);
            ViewBag.Title = "Authors";
            return View(new StaticPagedList<Author>(authors, page, perPage, _repository.Count<Author>()));
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
            if (!ModelState.IsValid)
            {
                return View(author);
            }

            if (IsAuthorExist(author.Name))
            {
                TempData["flashError"] = "An author with this name already exists";
                return View(author);
            }

            var createdAuthor = _repository.Create(author);
            return RedirectToAction("Details", "Authors", new {id = createdAuthor.Id});
        }

        private bool IsAuthorExist(string authorName)
        {
            return _repository.Search<Author>(a => a.Name == authorName).Any();
        }

        [HttpGet]
        [AllowAnonymous]
        public ViewResult Details(int id)
        {
            return View(_repository.Get<Author>(id));
        }

        [HttpGet]
        [Authorize(Roles = Roles.Admin)]
        public ViewResult Edit(int id)
        {
            return View(_repository.Get<Author>(id));
        }

        [HttpPut]
        [Authorize(Roles = Roles.Admin)]
        public ActionResult Edit(Author author)
        {
            if (!ModelState.IsValid )
            {
                return View(author);
            }
            if (IsAuthorNameUsedByAnotherAuthor(author))
            {
                TempData["flashError"] = "An author with this name already exists";
                return View(author);
            }
            _repository.Edit(author);
            return RedirectToAction("Details", "Authors", new {id = author.Id});
        }

        private bool IsAuthorNameUsedByAnotherAuthor(Author author)
        {
            return _repository.Search<Author>(a => a.Id != author.Id && a.Name == author.Name).Any();
        }
    }
}