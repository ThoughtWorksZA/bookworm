using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace BookWorm.Models.Validations
{
    public class ValidAgeRange : ValidationAttribute //, IClientValidatable
    {
        public List<string> ValidAgeRanges = new List<string>
            {
                "0-2",
                "3-5",
                "6-8",
                "9-12",
                "13-18"
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