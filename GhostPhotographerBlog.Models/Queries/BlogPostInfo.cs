using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GhostPhotographerBlog.Models.Queries
{
    public class BlogPostInfo
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string PostContent { get; set; }
        public string PostImage { get; set; }
        public string Author { get; set; }
        public string DisplayAuthor { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime ScheduleDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public List<string> Tags { get; set; }
    }
}
