using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MyPinterestVersion.Models
{
    public class Image
    {
        [Key]
        public int ImageID { get; set; }
        public string Url { get; set; }
        [DisplayName("Upload File")]
        public string ImagePath { get; set; }
        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }
    }
}