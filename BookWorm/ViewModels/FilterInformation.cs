using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookWorm.ViewModels
{
    public class FilterInformation
    {
        public List<string> Languages { get; set; }
        public List<BookInformation> BookInformations { get; set; }

        public FilterInformation(List<BookInformation> bookInformations)
        {
            Languages = new List<string>();
            BookInformations = bookInformations;
        }

        public FilterInformation(List<string> languages, List<BookInformation> bookInformations)
        {
            Languages = languages;
            BookInformations = bookInformations;
        }
    }
}