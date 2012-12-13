using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using BookWorm.Models.Validations;

namespace BookWorm.Models
{
    public class Book : Model<Book>
    {
        [Required]
        public string Title { get; set; }

        public string Subtitle { get; set; }

        [Required]
        public string Author { get; set; }

        public string Illustrator { get; set; }

        [Required]
        public string Editor { get; set; }

        [Required]
        [StringLength(13, MinimumLength = 9, ErrorMessage = "ISBN can only be 9 to 13 characters long.")]
        [RegularExpression("^([a-zA-Z0-9]+)$", ErrorMessage = "ISBN can only have letters and digits")]
        public string Isbn { get; set; }

        [Required]
        public string Publisher { get; set; }

        [Required]
        [ValidLanguage]
        public string Language { get; set; }


        [Required]
        public string Genre { get; set; }

        [DisplayName("Recommended Age Group")]
        [Required]
        [ValidAgeGroup]
        public string RecommendedAgeGroup { get; set; }

        [Required]
        [ValidCountry]
        public string Country { get; set; }

        [DisplayName("Cover Image")]
        public string CoverImageUrl { get; set; }

        public string Description { get; set; }
    }
}