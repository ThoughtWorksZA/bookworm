using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BookWorm.Models;

namespace BookWorm.ViewModels
{
    public interface IViewModel
    {
        Model Model { get; }
    }

    public interface IViewModel<T> : IViewModel
    {
        new T Model { get; set; }
    }

    public abstract class ViewModel<T> : IViewModel<T> where T : Model
    {
        Model IViewModel.Model { get { return Model; } }
        public abstract T Model { get; set; }
        public virtual string CreateSucceededMessage { get { return string.Format("Added {0}", Model.GetType().Name); } }
        public virtual string CreateFailedMessage { get { return string.Format("Sorry, {0} with id {1} already exists.", Model.GetType().Name, Model.Id); } }
        public virtual string UpdateSucceededMessage { get { return string.Format("Updated {0} successfully", Model.GetType().Name); } }
        public virtual string DeleteSucceededMessage { get { return string.Format("{0} successfully deleted", Model.GetType().Name); } }
    }
}