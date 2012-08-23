using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace MultiEditWithSignalR.Models
{
    public class BlogPostContext : DbContext
    {
        public DbSet<BlogPost> BlogPosts { get; set; }
    }
}