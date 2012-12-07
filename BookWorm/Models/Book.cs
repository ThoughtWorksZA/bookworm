using System.ComponentModel.DataAnnotations;

namespace BookWorm.Models
{
    public class Book : Model<Book>
    {
        [Required]
        public string Title { get; set; }

        public string SubTitle { get; set; }

        [Required]
        public string Author { get; set; }

        public string Illustrator { get; set; }
    }
}
