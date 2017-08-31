using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using GhostPhotographerBlog.UI.Helpers;

namespace GhostPhotographerBlog.UI.Models
{
    public class SearchModel
    {
        public SearchType SearchType { get; set; }
        public string SearchArg { get; set; }
    }
}