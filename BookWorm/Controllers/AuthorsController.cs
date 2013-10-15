using System.Linq;
using System.Web.Mvc;
using BookWorm.Models;
using BookWorm.ViewModels;
using PagedList;
using Raven.Client;

namespace BookWorm.Controllers
{
    public class AuthorsController : BaseController
    {
        private const int MaxNumberOfBooksInDetail = 4;

        public AuthorsController()
        {
        }

        public AuthorsController(Repository repository) : base(repository)
        {
        }

        public AuthorsController(IDocumentSession documentSession)
        {
            DocumentSession = documentSession;
        }

        [HttpGet]
        [AllowAnonymous]
        public ViewResult List(int page = 1, int perPage = 9)
        {
            var authors = Repository.List<Author>(page, perPage);
            ViewBag.Title = "Authors";
            return View(new StaticPagedList<Author>(authors, page, perPage, Repository.Count<Author>()));
        }

        [HttpGet]
        [Authorize(Roles = Roles.Admin)]
        public ViewResult Create()
        {
            return View(new Author());
        }

        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        public ActionResult Create(Author author)
        {
            if (!ModelState.IsValid)
            {
                return View(author);
            }

            if (IsAuthorExist(author.Name))
            {
                TempData["flashError"] = "An author with this name already exists";
                return View(author);
            }

            var createdAuthor = Repository.Create(author);
            return RedirectToAction("Details", "Authors", new {id = createdAuthor.Id});
        }

        private bool IsAuthorExist(string authorName)
        {
            return Repository.Search<Author>(a => a.Name == authorName).Any();
        }

        [HttpGet]
        [AllowAnonymous]
        public ViewResult Details(int id)
        {
            var author = Repository.Get<Author>(id);
            var allBooks = Repository.Search<Book>(b => b.Author == author.Name);
            var books = allBooks.Take(MaxNumberOfBooksInDetail).ToList();
            var authorViewModel = new AuthorViewModel(author, books, allBooks.Count > MaxNumberOfBooksInDetail);
            return View(authorViewModel);
        }

        [HttpGet]
        [Authorize(Roles = Roles.Admin)]
        public ViewResult Edit(int id)
        {
            return View(Repository.Get<Author>(id));
        }

        [HttpPut]
        [Authorize(Roles = Roles.Admin)]
        public ActionResult Edit(Author author)
        {
            if (!ModelState.IsValid )
            {
                return View(author);
            }
            if (IsAuthorNameUsedByAnotherAuthor(author))
            {
                TempData["flashError"] = "An author with this name already exists";
                return View(author);
            }
            Repository.Edit(author);
            return RedirectToAction("Details", "Authors", new {id = author.Id});
        }

        [HttpGet]
        [AllowAnonymous]
        public ViewResult Books(int id, int page = 1, int perPage = 9)
        {
            var author = Repository.Get<Author>(id);
            var books = Repository.Search<Book>(b => b.Author == author.Name);
            ViewBag.Author = author;
            return View(new StaticPagedList<Book>(books.Skip((page-1)*perPage).Take(perPage), page, perPage, books.Count));
        }

        private bool IsAuthorNameUsedByAnotherAuthor(Author author)
        {
            return Repository.Search<Author>(a => a.Id != author.Id && a.Name == author.Name).Any();
        }
    }
}
