using System.ComponentModel.DataAnnotations;

namespace BookWorm.Models
{
    public class BookPost : BasePost
    {
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
