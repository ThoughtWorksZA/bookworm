using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace BookWorm.Models.Validations
{
    public class ValidGenre : ValidationAttribute //, IClientValidatable
    {
        public static List<string> ValidGenres = new List<string>
            {
                "Picture Books",
                "Fiction",
                "Non-Fiction",
                "Poetry",
                "Rhymes and Riddles",
                "Fables and Folktales",
                "Educational",
                "Young Adult – Fantasy/Science Fiction",
                "Young Adult – Contemporary Fiction",
                "Young Adult – Historical Fiction"
            };

        public override bool IsValid(object genre)
        {
            return CheckSpecifiedLanguageIsValid(genre);
        }

        private bool CheckSpecifiedLanguageIsValid(object genre)
        {
            return ValidGenres.Contains(genre);
        }
   
    }
}