using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyPinterestVersion.Models
{
    public class SimilarUrl
    {
        [Key]
        public int Id { get; set; }

        public string UrlBody { get; set; }
        public int BookmarkId { get; set; }

        public virtual Bookmark Bookmark { get; set; }
        public DateTime Date { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}