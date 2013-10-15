using System.Collections.Generic;

namespace BookWorm.Models
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
