using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GhostPhotographerBlog.Models.Queries
{
    public class AddCommentParameters
    {
        public int PostId { get; set; }
        public string Comment { get; set; }
        public string UserId { get; set; }
    }
}
