using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookWorm.Models;
using BookWorm.Models.Validations;

namespace BookWorm.ViewModels
{
    public class BookInformation
    {
        public Book Book { get; set; }
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
            Book = book;
            BookPostInformations = new List<BookPostInformation>();
        }

        public BookInformation(Book book, List<BookPostInformation> bookPostInformations)
        {
            Book = book;
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
            return new SelectList(new ValidAgeRange().ValidAgeRanges);
        }

        public SelectList ValidCountries()
        {
            return new SelectList(new ValidCountry().ValidCountries);
        }

        private List<BookPostInformation> SpecificBookPostInformations(BookPost.BookPostType bookPostType)
        {
            return
                BookPostInformations.Where(
                    bookPostInformation => bookPostInformation.BookPost.Type.Equals(bookPostType))
                                    .ToList();
        }
    }
}