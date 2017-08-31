using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GhostPhotographerBlog.Models.Queries;

namespace GhostPhotographerBlog.UI.Models
{
    public class HashTagWeightViewModel : HashtagInfo
    {
        public int HashtagWeight { get; set; }
    }
}