using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace BookWorm.Models.Validations
{
    public class ValidCountry : ValidationAttribute //, IClientValidatable
    {
        public List<string> ValidCountries = new List<string>
            {
                    "Angola",
                    "Botswana",
                    "Egypt",
                    "Ethiopia",
                    "Ghana",
                    "Kenya",
                    "Lesotho",
                    "Liberia",
                    "Namibia",
                    "Nigeria",
                    "Rwanda",
                    "Swaziland",
                    "South Africa",
                    "Tanzania",
                    "Uganda",
                    "Zambia",
                    "Zimbabwe",
                    "International"
                };

        public override bool IsValid(object specifiedCountry)
        {
            return CheckSpecifiedCountryIsValid(specifiedCountry);
        }

        private bool CheckSpecifiedCountryIsValid(object specifiedCountry)
        {
            return ValidCountries.Contains(specifiedCountry);
        }
   
    }
}