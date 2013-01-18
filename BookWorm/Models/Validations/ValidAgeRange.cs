using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace BookWorm.Models.Validations
{
    public class ValidAgeRange : ValidationAttribute //, IClientValidatable
    {
        public static List<string> ValidAgeRanges = new List<string>
            {
                "0-2 years",
                "3-5 years",
                "6-8 years",
                "9-12 years",
                "13-18 years"
            };

        public override bool IsValid(object specifiedAgeRange)
        {
            return CheckSpecifiedAgeRangeIsValid(specifiedAgeRange);
        }

        private bool CheckSpecifiedAgeRangeIsValid(object specifiedAgeRange)
        {
            return ValidAgeRanges.Contains(specifiedAgeRange);
        }
    }
}