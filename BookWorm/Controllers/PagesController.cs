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
    }
}