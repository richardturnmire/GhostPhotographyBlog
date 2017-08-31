using GhostPhotographerBlog.Data;
using GhostPhotographerBlog.Data.Repositories;
using GhostPhotographerBlog.Models.Queries;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;

namespace GhostPhotographerBlog.Tests.IntegrationTests
{
    [TestFixture]
    public class DatabaseTests
    {
        // Class wide variables
        BlogRepository repo;
        int TestBlogPost = 7;
        int CommentCount = 3;  // Count of comments tied to TestBlogPost

        [SetUp]
        public void Init()
        {
            switch (Settings.GetMode())
            {
                case "Test":
                    Settings.SetConnectionString("TestingConnection");
                    break;
                case "Prod":
                    Settings.SetConnectionString("DefaultConnection");
                    break;
                default:
                    throw new Exception("Could not find valid Mode in App.config file.");
            }

            using (var conn = new SqlConnection(Settings.GetConnection()))
            {
                var cmd = new SqlCommand();
                cmd.CommandText = "DbReset";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Connection = conn;
                conn.Open();

                cmd.ExecuteNonQuery();
            }

            repo = new BlogRepository();

        }

        [Test]
        public void CanGetCommentsByPostId()
        {
            var comments = repo.GetCommentsByPostId(TestBlogPost);

            Assert.AreEqual(CommentCount, comments.Count);
        }

        [Test]
        public void CanAddComment()
        {
            // Verify adding a comment to a post and returning all
            // comments related to that post

            AddCommentParameters acp = new AddCommentParameters();

            acp.PostId = 13;
            acp.Comment = "Test comment to post 13";
            acp.UserId = "00000000-0000-0000-0000-000000000000";

            // Get current count of comments (before add)
            var comments = repo.GetCommentsByPostId(acp.PostId);
            int commentCount = comments.Count;

            // Add the comment to the blog post
            var comment = repo.NewPostComment(acp);

            // Get the new list of comments
            comments = repo.GetCommentsByPostId(acp.PostId);
            Assert.AreEqual(commentCount + 1, comments.Count);

            // Add another
            acp.Comment = "Test comment to post 13 from another user";
            acp.UserId = "11111111-1111-1111-1111-111111111111";
            comment = repo.NewPostComment(acp);

            // Get new count of comments and test
            comments = repo.GetCommentsByPostId(acp.PostId);
            Assert.AreEqual(commentCount + 2, comments.Count);

            // Add another
            acp.Comment = "Test comment to post 13 responding to previous comment";
            acp.UserId = "00000000-0000-0000-0000-000000000000";
            comment = repo.NewPostComment(acp);

            // Get new count of comments and test
            comments = repo.GetCommentsByPostId(acp.PostId);
            Assert.AreEqual(commentCount + 3, comments.Count);
        }

        [Test]
        public void CanDeleteComment()
        {
            // First add a comments to a post

            AddCommentParameters acp = new AddCommentParameters();

            acp.PostId = 13;
            acp.Comment = "Test comment #1 added to post 13";
            acp.UserId = "00000000-0000-0000-0000-000000000000";
            var comment = repo.NewPostComment(acp);

            acp.Comment = "Test comment #2 added to post 13 from another user";
            acp.UserId = "11111111-1111-1111-1111-111111111111";
            comment = repo.NewPostComment(acp);

            acp.Comment = "Test comment #3 adeed to post 13 responding to previous comment";
            acp.UserId = "00000000-0000-0000-0000-000000000000";
            comment = repo.NewPostComment(acp);

            // Get entire list of comments
            var comments = repo.GetCommentsByPostId(acp.PostId);
            int commentCount = comments.Count;

            // Find the most recent comment added
            int commentId = 0;
            foreach (var c in comments)
            {
                commentId = (c.Id > commentId) ? c.Id : commentId;
            }

            repo.DeleteCommentById(commentId);

            comments = repo.GetCommentsByPostId(acp.PostId);
            Assert.AreEqual(commentCount - 1, comments.Count);
        }

        [Test]
        public void CanGetPostsByHashtagId()
        {
            var posts = repo.GetPostsByHashtagId(1);

            Assert.AreEqual(10, posts.Count);
            Assert.AreEqual("Ghosts Are Very Scary!", posts[0].Title);
            Assert.AreEqual("The Scary Ghost Goes Boo", posts[1].Title);
        }

        [Test]
        public void CanGetHashtagInfo()
        {
            var hashtags = repo.GetHashtagInfo();

            // This needs to be re-written because order is not guaranteed
            Assert.AreEqual(9, hashtags.Count);
            Assert.AreEqual("#Galloway", hashtags[0].HashtagName);
            Assert.AreEqual(3, hashtags[0].Count);
            Assert.AreEqual("#Ghost", hashtags[1].HashtagName);
            Assert.AreEqual(10, hashtags[1].Count);
            Assert.AreEqual("#GhostPhoto", hashtags[2].HashtagName);
            Assert.AreEqual(1, hashtags[2].Count);
            Assert.AreEqual("#GhostPhotography", hashtags[3].HashtagName);
            Assert.AreEqual("#Photo", hashtags[4].HashtagName);
            Assert.AreEqual(7, hashtags[4].Count);
            Assert.AreEqual("#Photography", hashtags[5].HashtagName);
            Assert.AreEqual("#Scary", hashtags[6].HashtagName);
            Assert.AreEqual("#TrueStory", hashtags[7].HashtagName);
            Assert.AreEqual("#Waverly", hashtags[8].HashtagName);
            Assert.AreEqual(5, hashtags[8].Count);
        }

        [Test]
        public void CanGetPendingPosts()
        {
            var posts = repo.GetPendingPosts();

            Assert.AreEqual(6, posts.Count);
        }

        [Test]
        public void CanGetPageCount()
        {
            var pageCount = repo.GetApprovedPostsPageCount();

            Assert.AreEqual(3, pageCount);
        }

        [Test]
        public void CanGetPageOfApprovedPosts()
        {
            var posts = repo.GetPageOfApprovedPosts(1);
            Assert.AreEqual(10, posts.Count);

            posts = repo.GetPageOfApprovedPosts(2);
            Assert.AreEqual(10, posts.Count);

            posts = repo.GetPageOfApprovedPosts(3);
            Assert.AreEqual(4, posts.Count);

        }

        [Test]
        public void CanGetUsers()
        {
            var users = repo.GetAllUsers();
            Assert.AreEqual(4, users.Count);
        }

        [Test]
        public void CanGetApprovedPostById()
        {
            var post = repo.GetApprovedPostById(1);
            Assert.AreEqual(1, post.Id);
            Assert.AreEqual("Ghosts Are Very Scary!", post.Title);
        }

        [Test]
        public void CanCreateBlogPost()
        {
            CreatePostParameters cpp = new CreatePostParameters();

            cpp.PostType = "Blog";
            cpp.Title = "Ghosts also enjoy their morning cup";
            cpp.PostContent = "<p>I was watching this ghost drinking coffee and the mist was mystifying</p>";
            cpp.PostImage = "image_test.jpg";
            cpp.AuthorId = "00000000-0000-0000-0000-000000000000";
            cpp.DisplayAuthorId = "22222222-2222-2222-2222-222222222222";
            cpp.StatusName = "Approved";
            cpp.DateCreated = DateTime.Now;
            cpp.ScheduleDate = DateTime.Now;
            cpp.ExpirationDate = DateTime.Now.AddDays(30);
            cpp.Tags = new List<string>();
            cpp.Tags.Add("#Hashtag1");
            cpp.Tags.Add("#Ghost2");
            cpp.Tags.Add("#Haunting3");

            var postId = repo.CreateBlogPost(cpp);

            var post = repo.GetPostById(postId);

            Assert.AreEqual(cpp.Title, post.Title);
            Assert.IsNull(post.ModifiedDate);
        }

        [Test]
        public void CanEditBlogPost()
        {
            CreatePostParameters cpp = new CreatePostParameters();

            cpp.PostType = "Blog";
            cpp.Title = "Ghosts also enjoy their morning cup";
            cpp.PostContent = "<p>I was watching this ghost drinking coffee and the mist was mystifying</p>";
            cpp.PostImage = "image_test.jpg";
            cpp.AuthorId = "00000000-0000-0000-0000-000000000000";
            cpp.DisplayAuthorId = "22222222-2222-2222-2222-222222222222";
            cpp.StatusName = "Pending";
            cpp.DateCreated = DateTime.Now;
            cpp.ScheduleDate = DateTime.Now;
            cpp.ExpirationDate = DateTime.Now.AddDays(30);
            cpp.Tags = new List<string>();
            cpp.Tags.Add("#Hashtag1");
            cpp.Tags.Add("#Ghastly4");
            cpp.Tags.Add("#Haunting3");

            var postId = repo.CreateBlogPost(cpp);

            var post = repo.GetPostById(postId);

            Assert.AreEqual(cpp.StatusName, post.StatusName);

            EditPostParameters epp = new EditPostParameters();

            epp.Id = postId;
            epp.PostType = cpp.PostType;
            epp.Title = cpp.Title + " of Joe ghost";
            epp.PostContent = cpp.PostContent;
            epp.PostImage = cpp.PostImage;
            epp.AuthorId = cpp.AuthorId;
            epp.DisplayAuthorId = cpp.DisplayAuthorId;
            epp.StatusName = "Approved";
            epp.DateCreated = cpp.DateCreated;
            epp.ModifiedDate = DateTime.Now.AddSeconds(30);
            epp.ScheduleDate = cpp.ScheduleDate;
            epp.ExpirationDate = cpp.ExpirationDate;

            repo.EditBlogPost(epp);

            post = repo.GetPostById(postId);

            Assert.AreEqual(epp.Title, post.Title);
            Assert.AreNotEqual(cpp.Title, post.Title);
            Assert.AreEqual(epp.StatusName, post.StatusName);
            Assert.AreNotEqual(cpp.StatusName, post.StatusName);

            // Not changed
            Assert.AreEqual(cpp.PostContent, post.PostContent);
        }

        [Test]
        public void CanGetPostBody()
        {
            var postBody = repo.GetPostBodyById(3);

            Assert.AreEqual("<p>Aeromancy alectormancy ambulomancy anthracomancy brontomancy catoptromancy cephalonomancy coscinomancy crystalomancy eromancy geloscopy geomancy haruspication hieroscopy hypnomancy keraunoscopia logomancy narcomancy necyomancy odontomancy ololygmancy oryctomancy pyromancy rhapsodomancy scapulomancy sideromancy spheromancy tyromancy xenomancy. Ailuromancy axinomancy bibliomancy botanomancy brontomancy cartomancy chaomancy crithomancy critomancy hematomancy hieromancy kephalonomancy lithomancy logarithmancy meconomancy odontomancy ololygmancy omphalomancy onomancy onymancy oomancy ophidiomancy retromancy rhabdomancy scatoscopy sideromancy spheromancy thumomancy tiromancy topomancy. Anthracomancy crystalomancy knissomancy lithomancy logomancy metopomancy omoplatoscopy ouranomancy pyromancy spasmatomancy thumomancy.</p><p>Alectormancy belomancy cleidomancy crystallomancy ichnomancy lithomancy logomancy myomancy omoplatoscopy onomancy stolisomancy theomancy xylomancy. Acultomancy alectryomancy aleuromancy botanomancy ceraunoscopy cleidomancy critomancy knissomancy oinomancy psephomancy spatilomancy tephromancy trochomancy. Alectormancy austromancy belomancy causimancy ceraunoscopy dactyliomancy hydromancy kephalonomancy maculomancy meconomancy oinomancy ossomancy ouranomancy spatilomancy spheromancy thrioboly. Aleuromancy bibliomancy ceroscopy crystallomancy crystalomancy dririmancy haruspication hieroscopy lecanomancy logomancy oenomancy onymancy phyllomancy sortilege spheromancy urimancy. Arithmancy austromancy botanomancy brontomancy cleidomancy cleromancy grafology logomancy macromancy oenomancy pedomancy sideromancy stolisomancy.</p><p>Aleuromancy batraquomancy ceneromancy cleromancy lampadomancy narcomancy physiognomancy psephomancy scatoscopy spheromancy topomancy. Anthomancy batraquomancy brontomancy cleidomancy crystallomancy gyromancy lampadomancy metoposcopy spatilomancy spodomancy stolisomancy. Alomancy astromancy chiromancy cleromancy conchomancy graptomancy hieromancy hydromancy kephalonomancy ouranomancy pyromancy stercomancy. Anthomancy armomancy batraquomancy belomancy ceneromancy ceromancy chirognomy crithomancy critomancy hieromancy hydromancy logarithmancy myomancy odontomancy oneiromancy osteomancy pedomancy pegomancy scapulomancy stichomancy thumomancy urimancy xylomancy. Aleuromancy apantomancy armomancy botanomancy capnomancy catoptromancy coscinomancy critomancy cromnyomancy enoptromancy halomancy ichnomancy macromancy margaritomancy myomancy necyomancy nomancy ololygmancy oneiromancy onomancy onymancy oryctomancy ossomancy osteomancy retromancy spheromancy stolisomancy uranomancy.</p><p>Acultomancy astragalomancy cartomancy catoptromancy causimancy ceneromancy crithomancy dactyliomancy extispicy halomancy hepatoscopy ichthyomancy margaritomancy narcomancy nomancy osteomancy pedomancy pegomancy phyllomancy scatomancy. Aspidomancy austromancy bibliomancy ceraunoscopy cleromancy crithomancy crystallomancy dririmancy hematomancy ichnomancy keraunoscopia lecanomancy logarithmancy micromancy oinomancy ololygmancy omoplatoscopy stercomancy stolisomancy topomancy. Ailuromancy brontomancy chronomancy cleromancy ichthyomancy iconomancy mathemancy meconomancy ophidiomancy ouranomancy sciomancy selenomancy tiromancy. Axinomancy capnomancy ceneromancy ceraunomancy ceroscopy cromnyomancy crystallomancy dactyliomancy enoptromancy eromancy halomancy idolomancy knissomancy lithomancy margaritomancy meteoromancy metopomancy oinomancy onychomancy rhabdomancy spheromancy sycomancy theomancy.</p><p>These pants are stained!</p>", postBody);
        }
    }
}
