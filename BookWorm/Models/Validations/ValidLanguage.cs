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
        public List<string> _validLanguages = new List<string>
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
            return checkSpecifiedLanguageIsValid(specifiedLanguage);
        }

        private bool checkSpecifiedLanguageIsValid(object specifiedLanguage)
        {
            return _validLanguages.Contains(specifiedLanguage);
        }
   
    }
}