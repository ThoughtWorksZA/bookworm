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
        public ActionResult News(int page = 1, int perPage = 9, bool excludeDraft = true)
        {
            var isDraft = !excludeDraft;
            var eventsAndNews = GetEventsAndNews(isDraft);
            var books = _repository.List<Book>();
            var bookPosts = books.SelectMany(x => x.Posts.Where(bp => bp.Type == BookPost.BookPostType.News || bp.Type == BookPost.BookPostType.Events).Select(y => new BookPostInformation(x.Id, y, x))).ToList();
            var allPosts = new List<IBasePostInformation>();
            allPosts.AddRange(eventsAndNews);
            allPosts.AddRange(bookPosts);
            allPosts = allPosts.OrderByDescending(x => x.Model.UpdatedAt).ToList();
            return View(allPosts.ToPagedList(page, perPage));
        }

        private IEnumerable<PostInformation> GetEventsAndNews(bool isDraft)
        {
            if (!isDraft)
                    return _repository.List<Post>().Where(p => p.IsDraft == isDraft).Select(post => new PostInformation(post));
            
            return _repository.List<Post>()
                              .Where(p => p.IsDraft == isDraft && p.Creator.Equals(User.Identity.Name))
                              .Select(post => new PostInformation(post));
        }
    }
}
