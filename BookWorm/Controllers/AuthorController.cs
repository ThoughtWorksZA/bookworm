using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BirdBrain;
using BookWorm.Models;
using BookWorm.ViewModels;

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

        [HttpGet]
        public ViewResult List()
        {
            var authors = _session.Query<Author>().ToList();
            return View(authors);
        }

        [Authorize(Roles = Roles.Admin + "," + Roles.Author)]
        public ViewResult Create()
        {
            
            return View(new Author());
        }

    }
}
