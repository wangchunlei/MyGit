using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MultiEditWithSignalR.Models
{
    public class BlogPost
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(Name = "Article")]
        public string BlogBody { get; set; }
    }
}