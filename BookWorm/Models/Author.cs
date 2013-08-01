using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookWorm.Models
{
    public class Author : Model
    {
        [Required]
        public string Name { get; set; }
    }
}