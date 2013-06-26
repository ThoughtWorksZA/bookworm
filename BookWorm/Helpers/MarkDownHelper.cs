using System.Text.RegularExpressions;

namespace BookWorm.Helpers
{
    public class MarkDownHelper
    {
        private const string MatchImgPattern = @"\!\[[\w\s]+\]\[\d+\]";

        public static string Summary(string content, int characters)
        {
            var md = new MarkdownSharp.Markdown();
            if (content == null || content.Length < characters)
                return md.Transform(content);
            
            content = FilterImgs(content);

            var summary = GetSummaryPart(content, characters);

            return md.Transform(AddClosingAsterisk(RemoveEndingBracket(summary)));
        }

        private static string RemoveEndingBracket(string summary)
        {
            if (summary.EndsWith("\n>"))
            {
                summary = summary.Remove(summary.Length - 2);
            }
            return summary;
        }

        private static string AddClosingAsterisk(string summary)
        {
            if (summary.StartsWith("*") && summary.LastIndexOf("*") == 0)
            {
                summary = summary + "*";
            }
            return summary;
        }

        private static string GetSummaryPart(string content, int characters)
        {
            return content.Substring(0, content.IndexOf(" ", characters)).Trim();
        }

        private static string FilterImgs(string content)
        {
            var matchImgRegex = new Regex(MatchImgPattern);
            content = matchImgRegex.Replace(content, "");
            return content;
        }
    }
}