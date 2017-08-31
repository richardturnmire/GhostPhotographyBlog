using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GhostPhotographerBlog.Models.Queries
{
    public class Comments
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string Comment { get; set; }
        public DateTime CommentDate { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
    }
}
