namespace BookWorm.Helpers
{
    public class MarkDownHelper
    {
        public static string MarkDownSummary(string content, int characters)
        {
            var md = new MarkdownSharp.Markdown();
            if (content == null || content.Length < characters)
                return md.Transform(content);
            return md.Transform(content.Substring(0, content.IndexOf(" ", characters)));
        }
    }
}