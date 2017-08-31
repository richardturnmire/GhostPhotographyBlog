using GhostPhotographerBlog.Models.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GhostPhotographerBlog.UI.Models
{
    public class CommentViewModel
    {
        public CommentViewModel(Comments blogComment)
        {
            Id = blogComment.Id;
            PostId = blogComment.PostId;
            Comment = blogComment.Comment;
            CommentDate = blogComment.CommentDate;
            UserName = blogComment.UserName;
            DisplayName = blogComment.DisplayName;
        }

        public int Id { get; set; }
        public int PostId { get; set; }
        public string Comment { get; set; }
        public DateTime CommentDate { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }

        public Comments ConvertToModel()
        {
            Comments commented = new Comments
            {
                Id = Id,
                PostId = PostId,
                Comment = Comment,
                CommentDate = CommentDate,
                UserName = UserName,
                DisplayName = DisplayName
            };
            return commented;
        }

    }
}