using System.Linq;

namespace BookWorm.Helpers
{
    public class UrlUtils
    {
        private const string Invalid = @"<>*%&:\?/";
        public static string ConvertTitleForUrl(string content)
        {
            return Invalid.Aggregate(content, (current, c) => current.Replace(c.ToString(), string.Empty)).Replace("  "," ").Replace(" ","-");
        }

        public static string BuyBookContactHtml(string buyBookContactDetails)
        {
            if (buyBookContactDetails.ToLower().StartsWith("http"))
                return @"<a href=""" + buyBookContactDetails + @""">" + buyBookContactDetails + "</a>";
            if (buyBookContactDetails.ToLower().StartsWith("www"))
                return @"<a href=""http://" + buyBookContactDetails + @""">http://" + buyBookContactDetails +
                       "</a>";
            return "<em>" + buyBookContactDetails + "</em>";
        }
    }

    
}