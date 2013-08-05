using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace BookWorm.Models
{
    public class Author : Model
    {
        [Required]
        public string Name { get; set; }

        public string PictureUrl{ get; set; }

        [AllowHtml]
        public string Biography { get; set; }
    }
}