using BookWorm.Models;

namespace BookWorm.ViewModels
{
    public interface IBasePostInformation : IViewModel
    {
        new BasePost Model { get;  }
        string FeaturedImage { get; }
        string Summary(int characters);
        string DetailsUrl { get; }
    }

    public interface IBasePostInformation<T> : IViewModel<T>, IBasePostInformation where T : BasePost
    {
    }

    public class PostInformation : ViewModel<Post>, IBasePostInformation<Post>
    {
        public PostInformation()
        {
        }

        public PostInformation(Post post)
        {
            Model = post;
        }
        BasePost IBasePostInformation.Model { get { return Model; } }
        public string FeaturedImage { get { return Model.FeaturedImage; } }
        public override Post Model { get; set; }
        public string Content
        {
            get
            {
                var md = new MarkdownSharp.Markdown();
                return md.Transform(Model.Content);
            }
        }

        public string DetailsUrl { get { return string.Format("/Posts/Details/{0}", Model.Id); } }

        public string Summary(int characters)
        {
            var md = new MarkdownSharp.Markdown();
            if (Model.Content == null || Model.Content.Length < characters)
                return md.Transform(Model.Content);
            return md.Transform(Model.Content.Substring(0, Model.Content.IndexOf(" ", characters)));
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