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

        public string FilterUrl(string language)
        {
            var url = "";
            if (Languages.Contains(language))
            {
                url = string.Format("/Books/Filter?{0}", string.Join("&", Languages.Where(x => x != language).Select(x => "languages=" + HttpUtility.UrlEncode(x))));
            }
            else
            {
                url = string.Format("/Books/Filter?languages={0}&{1}", HttpUtility.UrlEncode(language), string.Join("&", Languages.Select(x => "languages=" + HttpUtility.UrlEncode(x))));
            }
            return url;
        }
    }
}