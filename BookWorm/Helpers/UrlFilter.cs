using System.Linq;

namespace BookWorm.Helpers
{
    public class UrlFilter
    {
        private const string Invalid = @"<>*%&:\?/";
        public static string FilterInvalidCharacters(string content)
        {
            return Invalid.Aggregate(content, (current, c) => current.Replace(c.ToString(), string.Empty));
        }
    }
}