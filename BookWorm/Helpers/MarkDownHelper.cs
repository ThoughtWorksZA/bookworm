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

            return md.Transform(GetUnTransformedSummary(content, characters));
        }

        private static string GetUnTransformedSummary(string content, int characters)
        {
            content = FilterImgs(content);

            var summary = GetSummaryPart(content, characters);

            return AddClosingAsterisk(RemoveEndingBracket(summary));
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
            var index = content.IndexOf(" ", characters);
            if (index == -1)
            {
                index = content.Length;
            }
            return content.Substring(0, index).Trim();
        }

        private static string FilterImgs(string content)
        {
            var matchImgRegex = new Regex(MatchImgPattern);
            content = matchImgRegex.Replace(content, "");
            return content;
        }

        public static string SummaryForMetaDescription(string content, int num)
        {
            return new Regex(@"[\*,_,\n,>]").Replace(GetUnTransformedSummary(content,num), "");
        }
    }
}