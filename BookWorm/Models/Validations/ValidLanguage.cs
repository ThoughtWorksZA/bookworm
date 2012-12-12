using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookWorm.Models.Validations
{
    public class ValidLanguage : ValidationAttribute //, IClientValidatable
    {
        public List<string> ValidLanguages = new List<string>
            {
                "Afrikaans",
                "English",
                "Ndebele",
                "Northern Sotho",
                "Sotho",
                "Swazi",
                "Tswana",
                "Tsonga",
                "Venda",
                "Xhosa",
                "Zulu"
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