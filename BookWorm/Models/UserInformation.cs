using System.Collections.Generic;
using System.Web.Mvc;
using BookWorm.Models;

namespace BookWorm.ViewModels
{
    public class UserInformation
    {
        public UserInformation()
        {
        }

        public UserInformation(RegisterModel model)
        {
            Model = model;
        }

        public RegisterModel Model { get; set; }

        public IEnumerable<string> ValidRoles()
        {
            return new List<string> {Roles.Admin, Roles.Author};
        }
    }
}