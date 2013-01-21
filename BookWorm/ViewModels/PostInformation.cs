using BookWorm.Models;

namespace BookWorm.ViewModels
{
    public class PostInformation : ViewModel<Post>
    {
        public override Post Model { get; set; }
        public string Content
        {
            get
            {
                var md = new MarkdownSharp.Markdown();
                return md.Transform(Model.Content);
            }
        }

        public override string CreateSucceededMessage
        {
            get { return string.Format("Added {0}", Model.Title); }
        }

        public override string CreateFailedMessage
        {
            get { return string.Format("Sorry, post {0} already exists.", Model.Title); }
        }

        public override string UpdateSucceededMessage
        {
            get { return string.Format("Updated {0} successfully", Model.Title); }
        }

        public override string DeleteSucceededMessage
        {
            get { return "Post successfully deleted"; }
        }
    }
}