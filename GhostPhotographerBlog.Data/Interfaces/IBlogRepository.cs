using GhostPhotographerBlog.Models.Queries;
using GhostPhotographerBlog.Models.Tables;
using System.Collections.Generic;

namespace GhostPhotographerBlog.Data.Interfaces
{
    public interface IBlogRepository
    {
        List<HashtagInfo> GetHashtagInfo();
        List<BlogPostInfo> GetPostsByHashtagId(int hashtagId);
        List<BlogPostInfo> GetPendingPosts();
        List<BlogPostInfo> GetPageOfApprovedPosts(int pageNum);
        List<BlogPostInfo> SearchPostsByAny(string searchString);
        List<BlogPostInfo> SearchPostsByTitle(string searchString);
        List<BlogPostInfo> SearchPostsByContent(string searchString);
        List<BlogPostInfo> SearchPostsByHashtag(string searchString);
        BlogPostInfo GetApprovedPostById(int postId);
        int GetApprovedPostsPageCount();
        List<Comments> GetCommentsByPostId(int postId);
        Comments NewPostComment(AddCommentParameters acp);
        Comments GetCommentById(int commentId);
        void DeleteCommentById(int commentId);
        List<UserRoles> GetAllUsers();
        BlogPostData GetPostById(int postId);
        int CreateBlogPost(CreatePostParameters cpp);
        void EditBlogPost(EditPostParameters epp);
        void RelateTagToPost(int postId, string hashtagName);
        string GetPostBodyById(int postId);
        List<Hashtag> GetHashtagsByPostId(int postId);
        void UpdateNewRegistration(string email);
    }
}
