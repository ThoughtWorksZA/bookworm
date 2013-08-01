using System.Collections.Generic;
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
                T savedModel = _repository.Create(viewModel.Model);
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
            return View(new U {Model = _repository.Get<T>(id)});
        }

        [HttpGet]
        [AllowAnonymous]
        public ViewResult List(int page = 1, int perPage = 9)
        {
            var viewModels = new List<U>();
            var models = _repository.List<T>(page, perPage);
            foreach (var model in models)
            {
                viewModels.Add(new U {Model = model});
            }
            return View(new StaticPagedList<U>(viewModels, page, perPage, _repository.Count<T>()));
        }

        [HttpDelete]
        [Authorize(Roles = Roles.Admin)]
        public RedirectToRouteResult Delete(int id)
        {
            var viewModel = new U();
            _repository.Delete<T>(id);
            TempData["flashSuccess"] = viewModel.DeleteSucceededMessage;
            return RedirectToAction("List");
        }

        [HttpGet]
        [Authorize(Roles = Roles.Admin)]
        public ViewResult Edit(int id)
        {
            return View(new U {Model = _repository.Get<T>(id)});
        }

        [HttpPut]
        [Authorize(Roles = Roles.Admin)]
        public RedirectToRouteResult Edit(U viewModel)
        {
            _repository.Edit(viewModel.Model);
            TempData["flashSuccess"] = viewModel.UpdateSucceededMessage;
            return RedirectToAction("Details", new { id = viewModel.Model.Id });
        }
    }
}
