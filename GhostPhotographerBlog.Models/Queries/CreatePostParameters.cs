using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GhostPhotographerBlog.Models.Queries
{
    public class CreatePostParameters
    {
        public int Id { get; set; }
        public string PostType { get; set; }
        public string Title { get; set; }
        public string PostContent { get; set; }
        public string PostImage { get; set; }
        public string AuthorId { get; set; }
        public string DisplayAuthorId { get; set; }
        public string StatusName { get; set; }
        public List<string> Tags { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime ScheduleDate { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}