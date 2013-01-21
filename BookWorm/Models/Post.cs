using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace BookWorm.Models
{
    public class Post : Model
    {
        [Required]
        [StringLength(140, ErrorMessage = "{0} must be between {2} and {1} characters long", MinimumLength = 3)]
        public string Title { get; set; }
        [Required]
        [AllowHtml]
        public string Content { get; set; }
        [Required]
        public string FeaturedImage { get; set; }
        [Required]
        public PostType? Type { get; set; }

        public enum PostType
        {
            News,
            Events
        }

     
    }
}