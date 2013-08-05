﻿using System;
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
            if (ModelState.IsValid)
            {
                var existingAuthor = _repository.Search<Author>(a => a.Name == author.Name).FirstOrDefault();

                if (existingAuthor != null)
                {
                    TempData["flashError"] = "An author with this name already exists";
                    ViewBag.Title = "Create Author";
                    return View(author);
                }

                var createdAuthor = _repository.Create(author);
                return RedirectToAction("Details", "Author", new { id = createdAuthor.Id }); ;
            }
            return View(author);
        }

        [HttpGet]
        [AllowAnonymous]
        public ViewResult Details(int id)
        {
            return View( _repository.Get<Author>(id) );
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
            if (ModelState.IsValid)
            {
                _repository.Edit(author);
                return RedirectToAction("Details", "Author", new { id = author.Id });
            }
            return View(author);
        }
    }
}
