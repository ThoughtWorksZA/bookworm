using System.ComponentModel.DataAnnotations;
using BookWorm.Repository;

namespace BookWorm.Models
{
    public class StaticPage : Model<StaticPage>
    {
        [Required]
        [StringLength(20, ErrorMessage = "{0} must be between {2} and {1} characters long", MinimumLength = 3)]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
    }
}
