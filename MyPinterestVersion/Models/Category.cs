using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyPinterestVersion.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Tag name is mandatory")]
        [StringLength(20, ErrorMessage = "Name can not be longer that 20 caracters")]
        public string Name { get; set; }      
      
    }
}