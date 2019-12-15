using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyPinterestVersion.Models
{
    public class Bookmark
    {

        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Titlu este obligatoriu")]
        [StringLength(20, ErrorMessage = "Titlul nu poate avea mai mult de 20 de caractere")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Descrierea este obligatorie")]
        public string Description { get; set; }        
        public int Note { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public int ImageId { get; set; }

        public virtual Image Image { get; set; }

    }
}