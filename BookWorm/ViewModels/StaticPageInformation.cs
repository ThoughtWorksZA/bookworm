using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BookWorm.Models;

namespace BookWorm.ViewModels
{
    public class StaticPageInformation : ViewModel<StaticPage>
    {
        public override StaticPage Model { get; set; }
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
            get { return string.Format("Sorry, page {0} already exists.", Model.Title); }
        }

        public override string UpdateSucceededMessage
        {
            get { return string.Format("Updated {0} successfully", Model.Title); }
        }

        public override string DeleteSucceededMessage
        {
            get { return "Page successfully deleted"; }
        }
    }
}