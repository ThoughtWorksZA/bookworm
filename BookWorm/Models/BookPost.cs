using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace BookWorm.Models
{
    public class BookPost : Model
    {
        [Required]
        public string Title { get; set; }
        
        [Required]
        [AllowHtml]
        public string Content { get; set; }

        [Required]
        public BookPostType? Type { get; set; }

        public enum BookPostType
        {
            Reviews,
            TeachingGuides,
            News,
            Events,
            Other
        }
    }
}