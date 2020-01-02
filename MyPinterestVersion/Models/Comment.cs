using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MyPinterestVersion.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "You can not comment without writing a comment")]
        [StringLength(20, ErrorMessage = "Comment can not be longer than 20 characters")]
        public string CommentBody { get; set; }
        public int BookmarkId { get; set; }

        public virtual Bookmark Bookmark { get; set; }
    }
}