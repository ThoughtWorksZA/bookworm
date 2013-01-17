using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookWorm.ViewModels
{
    public class FilterInformation
    {
        public List<string> Languages { get; set; }
        public List<string> AgeRanges { get; set; }
        public List<BookInformation> BookInformations { get; set; }

        public FilterInformation(List<BookInformation> bookInformations)
        {
            Languages = new List<string>();
            AgeRanges = new List<string>();
            BookInformations = bookInformations;
        }

        public FilterInformation(List<string> languages, List<string> ageRanges, List<BookInformation> bookInformations)
        {
            Languages = languages;
            AgeRanges = ageRanges;
            BookInformations = bookInformations;
        }

        public string LanguageFilterUrl(string language)
        {
            return FilterUrl("languages", language, Languages, new {Type = "ageRanges", Value = AgeRanges });
        }

        private string FilterUrl(string type, string value, List<string> primary, params dynamic[] secondaries)
        {
            string url;
            var secondaryValue = (List<string>)secondaries.First().Value;
            if (primary.Contains(value))
            {
                url = string.Format("/Books/Filter?{0}",
                                    string.Join("&",
                                                primary.Where(x => x != value)
                                                         .Select(x => string.Format("{0}={1}", type, HttpUtility.UrlEncode(x)))));
                if (secondaryValue.Any())
                    url += string.Format("&{0}",
                                     string.Join("&", secondaryValue.Select(x => string.Format("{0}={1}", secondaries.First().Type, HttpUtility.UrlEncode(x)))));
            }
            else
            {
                url = string.Format("/Books/Filter?{0}={1}", type, HttpUtility.UrlEncode(value));
                if (primary.Any())
                    url += "&" + string.Join("&", primary.Select(x => string.Format("{0}={1}", type, HttpUtility.UrlEncode(x))));
                if (secondaryValue.Any())
                    url += string.Format("&{0}",
                                     string.Join("&", secondaryValue.Select(x => string.Format("{0}={1}", secondaries.First().Type, HttpUtility.UrlEncode(x)))));
            }
            return url;
        }

        public string AgeRangeFilterUrl(string ageRange)
        {
            return FilterUrl("ageRanges", ageRange, AgeRanges, new { Type = "languages", Value = Languages });
        }
    }
}