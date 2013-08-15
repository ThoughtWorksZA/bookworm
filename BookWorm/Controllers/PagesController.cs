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

        [ActionName("BaseDelete")]
        public override RedirectToRouteResult Delete(int id)
        {
            return null;
        }

        [HttpDelete]
        [Authorize(Roles = Roles.Admin)]
        public RedirectToRouteResult Delete(int id, bool excludeDraft = true)
        {
            var viewModel = new StaticPageInformation();
            _repository.Delete<StaticPage>(id);
            TempData["flashSuccess"] = viewModel.DeleteSucceededMessage;
            var currentExcludeDraft = excludeDraft;
            return RedirectToAction("List", new{excludeDraft=currentExcludeDraft});
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
            var viewModels = BuildModel(page, perPage, isDraft);
            var allPages = _repository.Search<StaticPage>((p => p.IsDraft == isDraft));

            return View(new StaticPagedList<StaticPageInformation>(viewModels, page, perPage, allPages.Count));
        }

        private List<StaticPageInformation> BuildModel(int page, int perPage, bool isDraft)
        {
            var viewModels = new List<StaticPageInformation>();
            var pagesToDisplay = GetPagesToDisplay(page, perPage, isDraft);
            foreach (var staticPage in pagesToDisplay)
            {
                viewModels.Add(new StaticPageInformation {Model = staticPage});
            }
            return viewModels;
        }

        private IEnumerable<StaticPage> GetPagesToDisplay(int page, int perPage, bool isDraft)
        {
            if (isDraft)
               return _repository.Search<StaticPage>((p => p.IsDraft == isDraft && p.Creator.Equals(User.Identity.Name)),
                                                        page, perPage);
            
            return _repository.Search<StaticPage>((p => p.IsDraft == isDraft), page, perPage);
        }
    }
}