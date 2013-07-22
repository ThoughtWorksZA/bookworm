using System.Collections.Generic;
using System.Web.Mvc;
using BookWorm.Helpers;
using BookWorm.Models;
using BookWorm.ViewModels;
using System.Linq;
using PagedList;

namespace BookWorm.Controllers
{
    public class PostsController : SimpleController<Post, PostInformation>
    {
        public PostsController()
        {
        }

        public PostsController(Repository repository) : base(repository)
        {
        }

        [AllowAnonymous]
        public ActionResult News(int page = 1, int perPage = 9)
        {
            var posts = _repository.List<Post>().Select(post => new PostInformation(post));
            var books = _repository.List<Book>();
            var bookPosts = books.SelectMany(x => x.Posts.Where(bp => bp.Type == BookPost.BookPostType.News || bp.Type == BookPost.BookPostType.Events).Select(y => new BookPostInformation(x.Id, y, x))).ToList();
            var allPosts = new List<IBasePostInformation>();
            allPosts.AddRange(posts);
            allPosts.AddRange(bookPosts);
            allPosts = allPosts.OrderByDescending(x => x.Model.UpdatedAt).ToList();
            return View(allPosts.ToPagedList(page, perPage));
        }
    }
}
