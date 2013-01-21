using BookWorm.Models;
using BookWorm.ViewModels;

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
    }
}
