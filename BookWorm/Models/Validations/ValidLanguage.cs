using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace BookWorm.Models.Validations
{
    public class ValidLanguage : ValidationAttribute
    {
        public static List<string> ValidLanguages = new List<string>
            {
                "Afrikaans",
                "English",
                "isiNdebele",
                "Sepedi",
                "Sesotho",
                "Siswati",
                "Setswana",
                "Xitsonga",
                "Tshivenda",
                "isiXhosa",
                "isiZulu"
            };

        public override bool IsValid(object specifiedLanguage)
        {
            return CheckSpecifiedLanguageIsValid(specifiedLanguage);
        }

        private bool CheckSpecifiedLanguageIsValid(object specifiedLanguage)
        {
            return ValidLanguages.Contains(specifiedLanguage);
        }
   
    }
}
