using GhostPhotographerBlog.Models.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GhostPhotographerBlog.UI.Models
{
    public class BlogPostViewModel
    {

        public BlogPostViewModel(BlogPostInfo blog)
        {
            Id = blog.Id;
            Title = blog.Title;
            PostContent = blog.PostContent;
            PostImage = blog.PostImage;
            DisplayAuthor = blog.DisplayAuthor;
            DateCreated = blog.DateCreated;
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string PostContent { get; set; }
        public string PostImage { get; set; }
        public string DisplayAuthor { get; set; }
        public DateTime DateCreated { get; set; }
        public string NewComment { get; set; }
        public List<CommentViewModel> comments { get; set; }
        public string DateCreatedString { get; set; }
        public List<string>Tags { get; set; }

        public BlogPostInfo ConvertToModel()
        {
            BlogPostInfo blog = new BlogPostInfo()
            {
                Id = Id,
                Title = Title,
                PostContent = PostContent,
                PostImage = PostImage,
                DisplayAuthor = DisplayAuthor,
                DateCreated = DateCreated
                //DateCreatedString = DateCreatedString
            };
            return blog;
        }

        public IEnumerable<KeyValuePair<string, string>> SearchTypes { get; set; }
        public int SearchType { get; set; }
        public string SearchArg { get; set; }


    }
}