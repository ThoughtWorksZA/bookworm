using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BookWorm.Models;

namespace BookWorm.ViewModels
{
    public abstract class ViewModel<T> where T : Model
    {
        public abstract T Model { get; set; }
        public virtual string CreateSucceededMessage { get { return string.Format("Added {0}", Model.GetType().Name); } }
        public virtual string CreateFailedMessage { get { return string.Format("Sorry, {0} with id {1} already exists.", Model.GetType().Name, Model.Id); } }
        public virtual string UpdateSucceededMessage { get { return string.Format("Updated {0} successfully", Model.GetType().Name); } }
        public virtual string DeleteSucceededMessage { get { return string.Format("{0} successfully deleted", Model.GetType().Name); } }
    }
}