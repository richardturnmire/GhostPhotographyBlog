using GhostPhotographerBlog.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GhostPhotographerBlog.Models.Queries;
using GhostPhotographerBlog.Models.Tables;
using System.Data.SqlClient;
using System.Data;

namespace GhostPhotographerBlog.Data.Repositories
{
    public class BlogRepository : IBlogRepository
    {
        
        public List<Comments> GetCommentsByPostId(int postId)
        {
            List<Comments> comments = new List<Comments>();

            using (var conn = new SqlConnection(Settings.GetConnection()))
            {
                SqlCommand cmd = new SqlCommand("GetCommentsByPostId", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@PostId", postId);

                conn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Comments comment = new Comments();

                        comment.Id = (int)dr["Id"];
                        comment.PostId = (int)dr["PostId"];
                        comment.Comment = dr["Comment"].ToString();
                        comment.CommentDate = (DateTime)dr["CommentDate"];
                        comment.UserName = dr["UserName"].ToString();
                        comment.DisplayName = dr["DisplayName"].ToString();

                        comments.Add(comment);
                    }
                }
            }

            return comments;
        }

        public List<BlogPostInfo> GetPostsByHashtagId(int hashtagId)
        {
            List<BlogPostInfo> posts = new List<BlogPostInfo>();

            using (var conn = new SqlConnection(Settings.GetConnection()))
            {
                SqlCommand cmd = new SqlCommand("GetPostsByHashtagId", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@HashtagId", hashtagId);

                conn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        BlogPostInfo post = new BlogPostInfo();

                        post.Id = (int)dr["Id"];
                        post.Title = dr["Title"].ToString();
                        post.PostContent = dr["PostContent"].ToString();
                        post.PostImage = dr["PostImage"].ToString();
                        post.Author = dr["Author"].ToString();
                        post.DisplayAuthor = dr["DisplayAuthor"].ToString();
                        post.DateCreated = (DateTime)(dr["DateCreated"]);
                        post.ScheduleDate = (DateTime)dr["ScheduleDate"];

                        posts.Add(post);
                    }
                }
            }

            return posts;
        }

        public List<HashtagInfo> GetHashtagInfo()
        {
            List<HashtagInfo> hashtags = new List<HashtagInfo>();

            using (var conn = new SqlConnection(Settings.GetConnection()))
            {
                SqlCommand cmd = new SqlCommand("GetHashtagInfo", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        HashtagInfo hashtag = new HashtagInfo();
                        hashtag.HashtagName = dr["HashtagName"].ToString();
                        hashtag.Count = (int)dr["Count"];

                        hashtags.Add(hashtag);
                    }
                }
            }

            return hashtags;
        }

        public List<BlogPostInfo> GetPendingPosts()
        {
            List<BlogPostInfo> posts = new List<BlogPostInfo>();

            using (var conn = new SqlConnection(Settings.GetConnection()))
            {
                SqlCommand cmd = new SqlCommand("GetPendingPosts", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        BlogPostInfo post = new BlogPostInfo();
                        post.Id = (int)dr["Id"];
                        post.Title = dr["Title"].ToString();
                        post.PostContent = dr["PostContent"].ToString();
                        post.PostImage = dr["PostImage"].ToString();
                        post.Author = dr["Author"].ToString();
                        post.DisplayAuthor = dr["DisplayAuthor"].ToString();
                        post.DateCreated = (DateTime)(dr["DateCreated"]);
                        post.ScheduleDate = (DateTime)dr["ScheduleDate"];

                        posts.Add(post);
                    }
                }
            }

            return posts;
        }

        public Comments NewPostComment(AddCommentParameters acp)
        {
            Comments comment = null;

            using (var conn = new SqlConnection(Settings.GetConnection()))
            {
                SqlCommand cmd = new SqlCommand("NewPostComment", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@PostId", acp.PostId);
                cmd.Parameters.AddWithValue("@Comment", acp.Comment);
                cmd.Parameters.AddWithValue("@UserId", acp.UserId);

                conn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        comment = new Comments();

                        comment.Id = (int)dr["Id"];
                        comment.PostId = (int)dr["PostId"];
                        comment.Comment = dr["Comment"].ToString();
                        comment.CommentDate = (DateTime)dr["CommentDate"];
                        comment.UserName = dr["UserName"].ToString();
                        comment.DisplayName = dr["DisplayName"].ToString();
                    }
                }
            }

            return comment;
        }

        public void DeleteCommentById(int commentId)
        {
            using (var conn = new SqlConnection(Settings.GetConnection()))
            {
                SqlCommand cmd = new SqlCommand("DeleteCommentById", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Id", commentId);

                conn.Open();

                cmd.ExecuteNonQuery();
            }
        }

        public Comments GetCommentById(int commentId)
        {
            Comments comment = null;

            using (var conn = new SqlConnection(Settings.GetConnection()))
            {
                SqlCommand cmd = new SqlCommand("GetCommentById", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Id", commentId);

                conn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        comment = new Comments();

                        comment.Id = (int)dr["Id"];
                        comment.PostId = (int)dr["PostId"];
                        comment.Comment = dr["Comment"].ToString();
                        comment.CommentDate = (DateTime)dr["CommentDate"];
                        comment.UserName = dr["UserName"].ToString();
                        comment.DisplayName = dr["DisplayName"].ToString();
                    }
                }
            }

            return comment;
        }

        public List<BlogPostInfo> GetPageOfApprovedPosts(int pageNum)
        {
            List<BlogPostInfo> posts = new List<BlogPostInfo>();

            using (var conn = new SqlConnection(Settings.GetConnection()))
            {
                SqlCommand cmd = new SqlCommand("GetPageOfApprovedPosts", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@PageNum", pageNum);

                conn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        BlogPostInfo post = new BlogPostInfo();

                        post.Id = (int)dr["Id"];
                        post.Title = dr["Title"].ToString();
                        post.PostContent = dr["PostContent"].ToString();
                        post.PostImage = dr["PostImage"].ToString();
                        post.Author = dr["Author"].ToString();
                        post.DisplayAuthor = dr["DisplayAuthor"].ToString();
                        post.DateCreated = (DateTime)(dr["DateCreated"]);
                        post.ScheduleDate = (DateTime)dr["ScheduleDate"];

                        posts.Add(post);
                    }
                }
            }

            return posts;
        }

        public int GetApprovedPostsPageCount()
        {
            int pageCount = 0;

            using (var conn = new SqlConnection(Settings.GetConnection()))
            {
                SqlCommand cmd = new SqlCommand("GetApprovedPostsPageCount", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        pageCount = (int)dr["PageCount"];
                    }
                }
            }

            return pageCount;
        }

        public List<UserRoles> GetAllUsers()
        {
            List<UserRoles> users = new List<UserRoles>();

            using (var conn = new SqlConnection(Settings.GetConnection()))
            {
                conn.ConnectionString = Settings.GetConnection();
                SqlCommand cmd = new SqlCommand("GetAllUsers", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        UserRoles user = new UserRoles();

                        user.UserId = dr["UserId"].ToString();
                        user.UserName = dr["UserName"].ToString();
                        user.RoleId = dr["RoleId"].ToString();
                        user.RoleName = dr["RoleName"].ToString();

                        users.Add(user);
                    }
                }
            }

            return users;
        }

        public BlogPostInfo GetApprovedPostById(int postId)
        {
            BlogPostInfo post = null;

            using (var conn = new SqlConnection(Settings.GetConnection()))
            {
                SqlCommand cmd = new SqlCommand("GetApprovedPostById", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Id", postId);

                conn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        post = new BlogPostInfo();

                        post.Id = (int)dr["Id"];
                        post.Title = dr["Title"].ToString();
                        post.PostContent = dr["PostContent"].ToString();
                        post.PostImage = dr["PostImage"].ToString();
                        post.Author = dr["Author"].ToString();
                        post.DisplayAuthor = dr["DisplayAuthor"].ToString();
                        post.DateCreated = (DateTime)(dr["DateCreated"]);
                        post.ScheduleDate = (DateTime)dr["ScheduleDate"];
                    }
                }
            }

            return post;
        }

        public int CreateBlogPost(CreatePostParameters cpp)
        {
            using (var conn = new SqlConnection(Settings.GetConnection()))
            {
                SqlCommand cmd = new SqlCommand("CreateBlogPost", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter sp = new SqlParameter("@Id", SqlDbType.Int);
                sp.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(sp);

                cmd.Parameters.AddWithValue("@PostType", cpp.PostType);
                cmd.Parameters.AddWithValue("@Title", cpp.Title);
                cmd.Parameters.AddWithValue("@PostContent", cpp.PostContent);
                cmd.Parameters.AddWithValue("@PostImage", cpp.PostImage);
                cmd.Parameters.AddWithValue("@AuthorId", cpp.AuthorId);
                cmd.Parameters.AddWithValue("@DisplayAuthorId", cpp.DisplayAuthorId);
                cmd.Parameters.AddWithValue("@StatusName", cpp.StatusName);
                cmd.Parameters.AddWithValue("@DateCreated", cpp.DateCreated);
                cmd.Parameters.AddWithValue("@ScheduleDate", cpp.ScheduleDate);
                cmd.Parameters.AddWithValue("@ExpirationDate", cpp.ExpirationDate);

                conn.Open();

                cmd.ExecuteNonQuery();

                cpp.Id = (int)sp.Value;  // Just incase we decide to return the Id
            }

            // Now update hashtags associated with this post
            foreach (var hashtag in cpp.Tags)
            {
                RelateTagToPost(cpp.Id, hashtag);
            }

            return cpp.Id;
        }

        public void RelateTagToPost(int postId, string hashtagName)
        {
            using (var conn = new SqlConnection(Settings.GetConnection()))
            {
                SqlCommand cmd = new SqlCommand("RelateTagToPost", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@PostId", postId);
                cmd.Parameters.AddWithValue("@HashtagName", hashtagName);

                conn.Open();

                cmd.ExecuteNonQuery();
            }
        }

        public BlogPostData GetPostById(int postId)
        {
            BlogPostData post = null;

            using (var conn = new SqlConnection(Settings.GetConnection()))
            {
                SqlCommand cmd = new SqlCommand("GetPostById", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Id", postId);

                conn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        post = new BlogPostData();

                        post.Id = (int)dr["Id"];
                        post.PostType = dr["PostType"].ToString();
                        post.Title = dr["Title"].ToString();
                        post.PostContent = dr["PostContent"].ToString();
                        post.PostImage = dr["PostImage"].ToString();
                        post.Author = dr["Author"].ToString();
                        post.DisplayAuthor = dr["DisplayAuthor"].ToString();
                        post.StatusName = dr["StatusName"].ToString();
                        post.DateCreated = (DateTime)(dr["DateCreated"]);
                        if (dr["ModifiedDate"] != DBNull.Value)
                        {
                            post.ModifiedDate = (DateTime)(dr["ModifiedDate"]);
                        }
                        post.ScheduleDate = (DateTime)dr["ScheduleDate"];
                        if (dr["ExpirationDate"] != DBNull.Value)
                        {
                            post.ExpirationDate = (DateTime)dr["ExpirationDate"];
                        }
                    }
                }
            }

            return post;
        }

        public void EditBlogPost(EditPostParameters epp)
        {
            using (var conn = new SqlConnection(Settings.GetConnection()))
            {
                SqlCommand cmd = new SqlCommand("EditBlogPost", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Id", epp.Id);
                cmd.Parameters.AddWithValue("@PostType", epp.PostType);
                cmd.Parameters.AddWithValue("@Title", epp.Title);
                cmd.Parameters.AddWithValue("@PostContent", epp.PostContent);
                cmd.Parameters.AddWithValue("@PostImage", epp.PostImage);
                cmd.Parameters.AddWithValue("@AuthorId", epp.AuthorId);
                cmd.Parameters.AddWithValue("@DisplayAuthorId", epp.DisplayAuthorId);
                cmd.Parameters.AddWithValue("@StatusName", epp.StatusName);
                cmd.Parameters.AddWithValue("@DateCreated", epp.DateCreated);
                cmd.Parameters.AddWithValue("@ModifiedDate", epp.ModifiedDate);
                cmd.Parameters.AddWithValue("@ScheduleDate", epp.ScheduleDate);
                cmd.Parameters.AddWithValue("@ExpirationDate", epp.ExpirationDate);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

        }

        public string GetPostBodyById(int postId)
        {
            string postBody = null;

            using (var conn = new SqlConnection(Settings.GetConnection()))
            {
                SqlCommand cmd = new SqlCommand("GetPostBodyById", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Id", postId);

                conn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        postBody = dr["PostContent"].ToString();
                    }
                }
            }

            return postBody;
        }

        public List<BlogPostInfo> SearchPostsByAny(string searchString)
        {
            List<BlogPostInfo> posts = new List<BlogPostInfo>();

            using (var conn = new SqlConnection(Settings.GetConnection()))
            {
                SqlCommand cmd = new SqlCommand("SearchPostsByAny", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Search", searchString);

                conn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        BlogPostInfo post = new BlogPostInfo();

                        post.Id = (int)dr["Id"];
                        post.Title = dr["Title"].ToString();
                        post.PostContent = dr["PostContent"].ToString();
                        post.PostImage = dr["PostImage"].ToString();
                        post.Author = dr["Author"].ToString();
                        post.DisplayAuthor = dr["DisplayAuthor"].ToString();
                        post.DateCreated = (DateTime)(dr["DateCreated"]);
                        post.ScheduleDate = (DateTime)dr["ScheduleDate"];

                        posts.Add(post);
                    }
                }
            }

            return posts;
        }

        public List<BlogPostInfo> SearchPostsByTitle(string searchString)
        {
            List<BlogPostInfo> posts = new List<BlogPostInfo>();

            using (var conn = new SqlConnection(Settings.GetConnection()))
            {
                SqlCommand cmd = new SqlCommand("SearchPostsByTitle", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Search", searchString);

                conn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        BlogPostInfo post = new BlogPostInfo();

                        post.Id = (int)dr["Id"];
                        post.Title = dr["Title"].ToString();
                        post.PostContent = dr["PostContent"].ToString();
                        post.PostImage = dr["PostImage"].ToString();
                        post.Author = dr["Author"].ToString();
                        post.DisplayAuthor = dr["DisplayAuthor"].ToString();
                        post.DateCreated = (DateTime)(dr["DateCreated"]);
                        post.ScheduleDate = (DateTime)dr["ScheduleDate"];

                        posts.Add(post);
                    }
                }
            }

            return posts;
        }

        public List<BlogPostInfo> SearchPostsByContent(string searchString)
        {
            List<BlogPostInfo> posts = new List<BlogPostInfo>();

            using (var conn = new SqlConnection(Settings.GetConnection()))
            {
                SqlCommand cmd = new SqlCommand("SearchPostsByContent", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Search", searchString);

                conn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        BlogPostInfo post = new BlogPostInfo();

                        post.Id = (int)dr["Id"];
                        post.Title = dr["Title"].ToString();
                        post.PostContent = dr["PostContent"].ToString();
                        post.PostImage = dr["PostImage"].ToString();
                        post.Author = dr["Author"].ToString();
                        post.DisplayAuthor = dr["DisplayAuthor"].ToString();
                        post.DateCreated = (DateTime)(dr["DateCreated"]);
                        post.ScheduleDate = (DateTime)dr["ScheduleDate"];

                        posts.Add(post);
                    }
                }
            }

            return posts;
        }

        public List<BlogPostInfo> SearchPostsByHashtag(string searchString)
        {
            List<BlogPostInfo> posts = new List<BlogPostInfo>();

            using (var conn = new SqlConnection(Settings.GetConnection()))
            {
                SqlCommand cmd = new SqlCommand("SearchPostsByHashtag", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Search", searchString);

                conn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        BlogPostInfo post = new BlogPostInfo();

                        post.Id = (int)dr["Id"];
                        post.Title = dr["Title"].ToString();
                        post.PostContent = dr["PostContent"].ToString();
                        post.PostImage = dr["PostImage"].ToString();
                        post.Author = dr["Author"].ToString();
                        post.DisplayAuthor = dr["DisplayAuthor"].ToString();
                        post.DateCreated = (DateTime)(dr["DateCreated"]);
                        post.ScheduleDate = (DateTime)dr["ScheduleDate"];

                        posts.Add(post);
                    }
                }
            }

            return posts;
        }

        public List<Hashtag> GetHashtagsByPostId(int postId)
        {
            List<Hashtag> hashtags = new List<Hashtag>();

            using (var conn = new SqlConnection(Settings.GetConnection()))
            {
                SqlCommand cmd = new SqlCommand("GetHashtagsByPostId", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@PostId", postId);

                conn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Hashtag hashtag = new Hashtag();

                        hashtag.Id = (int)dr["Id"];
                        hashtag.HashtagName = dr["HashtagName"].ToString();

                        hashtags.Add(hashtag);
                    }
                }
            }

            return hashtags;
        }

        public void UpdateNewRegistration(string email)
        {
            using (var conn = new SqlConnection(Settings.GetConnection()))
            {
                SqlCommand cmd = new SqlCommand("UpdateNewRegistration", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Email", email);

                conn.Open();

                cmd.ExecuteNonQuery();
            }

        }
    }
}
