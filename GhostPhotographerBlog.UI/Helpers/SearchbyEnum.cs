using GhostPhotographerBlog.UI.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GhostPhotographerBlog.UI.Helpers
{
    public enum SearchType
    {
        [Display(Name = "All")]
        All = 0,
        [Display(Name = "Title")]
        Title = 1,
        [Display(Name = "Content")]
        Content = 2,
        [Display(Name = "Tag")]
        Tag = 3
    }
}