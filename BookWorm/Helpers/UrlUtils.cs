using System.Text.RegularExpressions;

namespace BookWorm.Helpers
{
    public class UrlUtils
    {
        private static readonly Regex SpacesRegex = new Regex(@"\s+");
        private static readonly Regex MultiHyphens = new Regex(@"\-+");
        private static readonly Regex InvalidCharactersRegex = new Regex(@"[<,>,*,%,&,:,\\,?,/,\,,.,(,)]");

        public static string ConvertTitleForUrl(string content)
        {
            return MultiHyphens.Replace(
                SpacesRegex.Replace(InvalidCharactersRegex.Replace(content.Trim(), string.Empty), "-"), "-");
        }

        public static string BuyBookContactHtml(string buyBookContactDetails)
        {
            if (buyBookContactDetails.ToLower().StartsWith("http"))
            {
                return string.Format(@"<a href=""{0}"">{0}</a>",buyBookContactDetails);
            }
            if (buyBookContactDetails.ToLower().StartsWith("www"))
            {
                return string.Format(@"<a href=""http://{0}"">http://{0}</a>",buyBookContactDetails);
            }
            return string.Format("<em>{0}</em>",buyBookContactDetails);
        }
    }
}
