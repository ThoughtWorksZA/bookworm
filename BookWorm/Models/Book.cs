using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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
        public string Isbn { get; set;}

        [Required]
        public string Publisher { get; set; }

        [Required]
        [ValidLanguage]
        public string Language { get; set; }

        [Required]
        public string Genre { get; set; }

        [DisplayName("Recommended Age Group")]
        [Required]
        public string RecommendedAgeGroup { get; set; }

        [Required]
        public string Country { get; set; }

        [DisplayName("Cover Image")]
        public string CoverImageUrl { get; set; }

        public string Description { get; set; }
    }
}
