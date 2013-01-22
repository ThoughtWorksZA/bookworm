using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookWorm.Models;
using BookWorm.ViewModels;

namespace BookWorm.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController()
        {
        }

        public HomeController(Repository repository) : base(repository)
        {
        }

        public ActionResult Index()
        {
            var books = _repository.List<Book>(4).Select(x => new BookInformation(x)).ToList();
            var posts = _repository.List<Post>(5).Select(x => new PostInformation {Model = x});
            var bookPosts = _repository.List<Book>()
                .SelectMany(x => x.Posts.Where(bp => bp.Type == BookPost.BookPostType.News || bp.Type == BookPost.BookPostType.Events)
                    .Select(y => new BookPostInformation(x.Id, y, x))).ToList();
            var allPosts = new List<IBasePostInformation>();
            allPosts.AddRange(posts);
            allPosts.AddRange(bookPosts);
            allPosts = allPosts.OrderByDescending(x => x.Model.UpdatedAt).Take(5).ToList();
            return View(new HomeViewModel {Books = books, News = allPosts});
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
