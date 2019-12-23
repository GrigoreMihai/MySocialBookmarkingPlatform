using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyPinterestVersion.Models
{
    public class CategoryUserBookmarkLink
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public int BookmarkId { get; set; }
        public virtual Bookmark Bookmark { get; set; }
    }
}