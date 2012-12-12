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

        public BookInformation(Book book)
        {
            this.Book = book;
        }

        public BookInformation()
        {
        }

        public SelectList ValidLanguages()
        {
            return new SelectList(new ValidLanguage().ValidLanguages);
        }

        public SelectList ValidAgeGroups()
        {
            return new SelectList(new List<string>() {"0-2", "3-5", "6-8", "9-12", "13-18"});
        }

        public SelectList ValidCountries()
        {
            return new SelectList(new List<string>()
                {
                    "Angola",
                    "Botswana",
                    "Egypt",
                    "Ethiopia",
                    "Ghana",
                    "Kenya",
                    "Lesotho",
                    "Liberia",
                    "Namibia",
                    "Nigeria",
                    "Rwanda",
                    "Swaziland",
                    "South Africa",
                    "Tanzania",
                    "Uganda",
                    "Zambia",
                    "Zimbabwe",
                    "International"
                });
        }
    }
}