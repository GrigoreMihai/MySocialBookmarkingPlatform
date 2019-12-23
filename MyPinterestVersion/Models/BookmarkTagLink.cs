using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyPinterestVersion.Models
{
    public class BookmarkTagLink
    {
        [Key]
        public int Id { get; set; }
        public int TagId { get; set; }
        public virtual Tag Tag { get; set; }
        public int BookmarkId { get; set; }
        public virtual Bookmark Bookmark { get; set; }
    }
}