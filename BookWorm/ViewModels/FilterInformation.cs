using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace BookWorm.ViewModels
{
    public class FilterInformation
    {
        public List<string> Languages { get; set; }
        public List<string> AgeRanges { get; set; }
        public List<string> Genres { get; set; }
        public List<string> PostTypes { get; set; }
        public IPagedList<BookInformation> BookInformations { get; set; }


        public FilterInformation(IPagedList<BookInformation> bookInformations)
        {
            Languages = new List<string>();
            AgeRanges = new List<string>();
            Genres = new List<string>();
            PostTypes = new List<string>();
            BookInformations = bookInformations;
        }

        public FilterInformation(List<string> languages, List<string> ageRanges, List<string> genres, List<string> postTypes, IPagedList<BookInformation> bookInformations)
        {
            Languages = languages;
            AgeRanges = ageRanges;
            Genres = genres;
            PostTypes = postTypes;
            BookInformations = bookInformations;
        }

        public string LanguageFilterUrl(string language)
        {
            return FilterUrl("languages", language, Languages, new { Type = "ageRanges", Value = AgeRanges }, new { Type = "genres", Value = Genres }, new { Type = "postTypes", Value = PostTypes });
        }

        public string AgeRangeFilterUrl(string ageRange)
        {
            return FilterUrl("ageRanges", ageRange, AgeRanges, new { Type = "languages", Value = Languages }, new { Type = "genres", Value = Genres }, new { Type = "postTypes", Value = PostTypes });
        }

        public string GenreFilterUrl(string genre)
        {
            return FilterUrl("genres", genre, Genres, new { Type = "languages", Value = Languages }, new { Type = "ageRanges", Value = AgeRanges }, new { Type = "postTypes", Value = PostTypes });
        }

        public string PostTypeFilterUrl(string postType)
        {
            return FilterUrl("postTypes", postType, PostTypes, new { Type = "languages", Value = Languages }, new { Type = "ageRanges", Value = AgeRanges }, new {Type = "genres", Value = Genres});
        }

        private string FilterUrl(string type, string value, List<string> primary, params dynamic[] secondaries)
        {
            string url;
            if (primary.Contains(value))
            {
                url = string.Format("/Books/Filter?{0}",
                    string.Join("&", primary.Where(x => x != value).Select(values => BuildQueryUrl(type, values))));
            }
            else
            {
                url = string.Format("/Books/Filter?{0}={1}", type, HttpUtility.UrlEncode(value));
                if (primary.Any())
                    url += "&" + string.Join("&", primary.Select(values => BuildQueryUrl(type, values)));
            }

            url += BuildSecondaryFilter(secondaries);

            return url;
        }

        private string BuildSecondaryFilter(params dynamic[] secondaries)
        {
            string url = "";
            foreach (var secondary in secondaries)
            {
                var secondaryValue = (List<string>)secondary.Value;
                if (secondaryValue.Any())
                    url += string.Format("&{0}",
                        string.Join("&", secondaryValue.Select(values => BuildQueryUrl(secondary.Type, values))));
            }
            return url;
        }

        private static dynamic BuildQueryUrl(string type, string values)
        {
            return string.Format("{0}={1}", type, HttpUtility.UrlEncode(values));
        }

        public string PaginationUrl(string action, int page, string searchQuery = null)
        {
            var paginationUrl = string.Format("/Books/{0}?page={1}{2}", action, page, BuildSecondaryFilter(new { Type = "genres", Value = Genres }, new { Type = "languages", Value = Languages }, new { Type = "ageRanges", Value = AgeRanges }, new { Type = "postTypes", Value = PostTypes }));
            if (!string.IsNullOrEmpty(searchQuery))
            {
                paginationUrl = string.Format("{0}&searchQuery={1}", paginationUrl, searchQuery);
            }
            return paginationUrl;
        }
    }
}