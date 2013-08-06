using System.Web.Mvc;
using BookWorm.Models;
using BookWorm.ViewModels;
using MarkdownSharp;
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
    }
}