using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BookWorm.Models;
using BookWorm.Models.Validations;

namespace BookWorm.ViewModels
{
    public class BookInformation : ViewModel<Book>
    {
        public override Book Model { get; set; }
        public List<BookPostInformation> BookPostInformations { get; set; }
        public List<BookPostInformation> Reviews 
        { 
            get {
                return SpecificBookPostInformations(BookPost.BookPostType.Reviews);
            }
        }

        public List<BookPostInformation> Events
        {
            get
            {
                return SpecificBookPostInformations(BookPost.BookPostType.Events);
            }
        }

        public List<BookPostInformation> News
        {
            get
            {
                return SpecificBookPostInformations(BookPost.BookPostType.News);
            }
        }

        public List<BookPostInformation> TeachingGuides
        {
            get
            {
                return SpecificBookPostInformations(BookPost.BookPostType.TeachingGuides);
            }
        }

        public List<BookPostInformation> Other
        {
            get
            {
                return SpecificBookPostInformations(BookPost.BookPostType.Other);
            }
        }

        public BookInformation(Book book)
        {
            Model = book;
            BookPostInformations = new List<BookPostInformation>();
        }

        public BookInformation(Book book, List<BookPostInformation> bookPostInformations)
        {
            Model = book;
            BookPostInformations = bookPostInformations;
        }

        public BookInformation()
        {
        }

        public SelectList ValidLanguages()
        {
            return new SelectList(ValidLanguage.ValidLanguages);
        }

        public SelectList ValidAgeGroups()
        {
            return new SelectList(ValidAgeRange.ValidAgeRanges);
        }

        public SelectList ValidGenres()
        {
            return new SelectList(ValidGenre.ValidGenres);
        }

        public SelectList ValidCountries()
        {
            return new SelectList(new ValidCountry().ValidCountries);
        }

        private List<BookPostInformation> SpecificBookPostInformations(BookPost.BookPostType bookPostType)
        {
            return
                BookPostInformations.Where(
                    bookPostInformation => bookPostInformation.Model.Type.Equals(bookPostType))
                                    .ToList();
        }

        public string Summary(int characters)
        {
            if (Model.Description == null || Model.Description.Length < characters)
                return Model.Description;
            return Model.Description.Substring(0, Model.Description.IndexOf(" ", characters));
        }
    }
}