using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookWorm.Models.Validations
{
    public class ValidRole : ValidationAttribute
    {
        public static List<string> ValidRoles = new List<string>()
            {
                "admin"
            };

        public override bool IsValid(object specifiedRole)
        {
            return CheckSpecifiedRoleIsValid(specifiedRole);
        }

        private static bool CheckSpecifiedRoleIsValid(object specifiredRole)
        {
            return ValidRoles.Contains(specifiredRole);
        }

    }


}