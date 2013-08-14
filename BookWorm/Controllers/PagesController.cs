using System.Collections.Generic;
using System.Web.Mvc;
using BookWorm.Models;
using BookWorm.ViewModels;
using MarkdownSharp;
using PagedList;
using Raven.Client.Exceptions;

namespace BookWorm.Controllers
{
    [Authorize]
    public class PagesController : SimpleController<StaticPage, StaticPageInformation>
    {
        public PagesController()
        {
        }

        public PagesController(Repository repository) : base(repository)
        {
        }

        [HttpGet]
        [Authorize(Roles = Roles.Admin)]
        public override ActionResult Create()
        {
            return base.Create();
        }

        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        public override ActionResult Create(StaticPageInformation viewModel)
        {
            return base.Create(viewModel);
        }

        [HttpGet]
        [Authorize(Roles = Roles.Admin)]
        public override ViewResult Edit(int id)
        {
            return base.Edit(id);
        }

        [HttpPut]
        [Authorize(Roles = Roles.Admin)]
        public override RedirectToRouteResult Edit(StaticPageInformation viewModel)
        {
            return base.Edit(viewModel);
        }

        [ActionName("OldList")]
        public override ViewResult List(int page = 1, int perPage = 9)
        {
            return null;
        }

        [HttpGet]
        [AllowAnonymous]
        public ViewResult List(int page = 1, int perPage = 9, bool excludeDraft = true)
        {
            var isDraft = !excludeDraft;
            var viewModels = new List<StaticPageInformation>();
            var models = _repository.Search<StaticPage>((p => p.IsDraft == isDraft), page, perPage);
            var allPages = _repository.Search<StaticPage>((p => p.IsDraft == isDraft));

            foreach (var model in models)
            {
                viewModels.Add(new StaticPageInformation { Model = model });
            }
            return View(new StaticPagedList<StaticPageInformation>(viewModels, page, perPage, allPages.Count));
        }
    }
}