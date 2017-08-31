using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GhostPhotographerBlog.Models.Tables
{
    public class BlogPost
    {
        public int Id { get; set; }
        public string PostType { get; set; }
        public string Title { get; set; }
        public string PostContent { get; set; }
        public string PostImage { get; set; }
        public string AuthorId { get; set; }
        public string DisplayAuthorId { get; set; }
        public int PostStatusId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime ModifiedDate { get; set; }
        public DateTime ScheduleDate { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
