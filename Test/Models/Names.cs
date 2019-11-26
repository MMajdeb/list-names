using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Test.Models
{
    public class Names
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Nom")]
        public string Name { get; set; }
    }
}
