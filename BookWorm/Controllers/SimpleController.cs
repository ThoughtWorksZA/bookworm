﻿using System.Collections.Generic;
using System.Web.Mvc;
using BookWorm.Models;
using BookWorm.ViewModels;
using Raven.Client.Exceptions;
using PagedList;

namespace BookWorm.Controllers
{
    public class SimpleController<T, U> : BaseController where T : Model, new() where U : ViewModel<T>, new()
    {
        public SimpleController()
        {
        }

        public SimpleController(Repository repository) : base(repository)
        {
        }

        [HttpGet]
        [Authorize(Roles = Roles.Admin + "," + Roles.Author)]
        public virtual ActionResult Create()
        {
            return View("Create", new U {Model = new T()});
        }

        [HttpPost]
        [Authorize(Roles = Roles.Admin + "," + Roles.Author)]
        public virtual ActionResult Create(U viewModel)
        {
            try
            {
                T savedModel = Repository.Create(viewModel.Model);
                TempData["flashSuccess"] = viewModel.CreateSucceededMessage;
                return RedirectToAction("Details", new {id = savedModel.Id});
            }
            catch (NonUniqueObjectException ex)
            {
                TempData["flashError"] = viewModel.CreateFailedMessage;
                return View();
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public ViewResult Details(int id)
        {
            return View(new U {Model = Repository.Get<T>(id)});
        }

        [HttpGet]
        [AllowAnonymous]
        public virtual ViewResult List(int page = 1, int perPage = 9)
        {
            var viewModels = new List<U>();
            var models = Repository.List<T>(page, perPage);
            foreach (var model in models)
            {
                viewModels.Add(new U {Model = model});
            }
            return View(new StaticPagedList<U>(viewModels, page, perPage, Repository.Count<T>()));
        }

        [HttpDelete]
        [Authorize(Roles = Roles.Admin)]
        public virtual RedirectToRouteResult Delete(int id)
        {
            var viewModel = new U();
            Repository.Delete<T>(id);
            TempData["flashSuccess"] = viewModel.DeleteSucceededMessage;
            return RedirectToAction("List");
        }

        [HttpGet]
        [Authorize(Roles = Roles.Admin + "," + Roles.Author)]
        public virtual ViewResult Edit(int id)
        {
            return View(new U {Model = Repository.Get<T>(id)});
        }

        [HttpPut]
        [Authorize(Roles = Roles.Admin + "," + Roles.Author)]
        public virtual RedirectToRouteResult Edit(U viewModel)
        {
            Repository.Edit(viewModel.Model);
            TempData["flashSuccess"] = viewModel.UpdateSucceededMessage;
            return RedirectToAction("Details", new { id = viewModel.Model.Id });
        }
    }
}
