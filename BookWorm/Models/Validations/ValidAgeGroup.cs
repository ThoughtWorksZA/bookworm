using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace BookWorm.Models.Validations
{
    public class ValidAgeGroup : ValidationAttribute //, IClientValidatable
    {
        public List<string> ValidAgeGroups = new List<string>
            {
                "0-2",
                "3-5",
                "6-8",
                "9-12",
                "13-18"
            };

        public override bool IsValid(object specifiedAgeGroup)
        {
            return CheckSpecifiedAgeGroupIsValid(specifiedAgeGroup);
        }

        private bool CheckSpecifiedAgeGroupIsValid(object specifiedAgeGroup)
        {
            return ValidAgeGroups.Contains(specifiedAgeGroup);
        }
    }
}