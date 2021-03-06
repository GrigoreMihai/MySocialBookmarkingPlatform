﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
        [StringLength(20, ErrorMessage = "Titlul nu poate avea mai mult de 20 de caractere")]
        public string Description { get; set; }        
        public int Note { get; set; }          
        public DateTime Date { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public int ImageId { get; set; }        
        public virtual Image Image { get; set; }
        [NotMapped]
        public List<String> TagsNames { get; set; }
        [NotMapped]
        public List<SelectListItem> Tags { get; set; }       
        [NotMapped]
        public string Comment { get; set; }
        [NotMapped]
        public List<Comment> CommentsList { get; set; }
        [NotMapped]
        public DateTime CommentDate { get; set; }
        [NotMapped]
        public List<SimilarUrl> SimilarUrls { get; set; }
        [NotMapped]
        public DateTime SimilarUrlDate { get; set; }
        [NotMapped]
        public string Url { get; set; }
        [NotMapped]
        public string CategoryName { get; set; }

    }
}