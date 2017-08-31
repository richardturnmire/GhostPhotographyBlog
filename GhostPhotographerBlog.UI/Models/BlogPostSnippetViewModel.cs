using GhostPhotographerBlog.UI.Helpers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GhostPhotographerBlog.UI.Models
{
    public class BlogPostSnippetViewModel
    {
        public IEnumerable<BlogPostViewModel> Snippets { get; set; }

        [Required(ErrorMessage = "Please select search option")]
        public SearchType? SearchType { get; set; }

        public int BlogPostPageCount { get; set; }

        [Required(ErrorMessage = "Entry required")]
        public string SearchArg { get; set; }

        public IEnumerable<HashTagWeightViewModel> HashTags { get; set; }
    }
}