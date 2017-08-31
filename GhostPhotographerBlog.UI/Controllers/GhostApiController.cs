using GhostPhotographerBlog.Data.Factory;
using GhostPhotographerBlog.Models.Queries;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Http;

namespace GhostPhotographerBlog.UI.Controllers
{
    public class GhostApiController : ApiController
    {
        bool debug = true;

        [Route("api/users")]
        [AcceptVerbs("GET")]
        public IHttpActionResult GetUsers() {
            var repo = BlogRespositoryFactory.GetMode();
            return Ok(repo.GetAllUsers());
        }


        [Route("api/approve/{id}")]
        [AcceptVerbs("POST")]
        public IHttpActionResult ApproveBlogPost(int id) {
            EditPostParameters tmp = new EditPostParameters();
            var repo = BlogRespositoryFactory.GetMode();
            var post = repo.GetPostById(id);
            post.StatusName = "Approved";
            repo.EditBlogPost(PostToPost(post));
            return Ok();
        }
        [Route("api/delete/{id}")]
        [AcceptVerbs("POST")]
        public IHttpActionResult DeleteBlogPost(int id) {
            EditPostParameters tmp = new EditPostParameters();
            var repo = BlogRespositoryFactory.GetMode();
            var post = repo.GetPostById(id);
            post.StatusName = "Expired";
            repo.EditBlogPost(PostToPost(post));
            return Ok();
        }
        private EditPostParameters PostToPost(BlogPostData post) {
            EditPostParameters tmp = new EditPostParameters(){
                Id = post.Id,
                PostType = post.PostType,
                Title = post.Title,
                PostContent = post.PostContent,
                PostImage = post.PostImage,
                AuthorId = "",
                DisplayAuthorId = "",
                StatusName = post.StatusName,
                DateCreated = post.DateCreated,
                ModifiedDate = DateTime.Now,
                ScheduleDate = post.ScheduleDate,
                ExpirationDate = (DateTime)post.ExpirationDate
            };
            var repo = BlogRespositoryFactory.GetMode();
            var contrib = (from user in repo.GetAllUsers()
                           where user.UserName == post.Author
                           select user)
                           .ToList()
                           .FirstOrDefault();
            tmp.AuthorId = contrib.UserId;
            contrib = (from user in repo.GetAllUsers()
                       where user.UserName == post.DisplayAuthor
                       select user)
                       .ToList()
                       .FirstOrDefault();
            tmp.DisplayAuthorId = contrib.UserId;
            return tmp;
        }

        [Route("api/edit/{id}")]
        [AcceptVerbs("POST")]
        public IHttpActionResult EditBlogPost(BlogPostData post) {
            EditPostParameters tmp = new EditPostParameters() {
                Id = post.Id,
                PostType = post.PostType,
                Title = post.Title,
                PostContent = post.PostContent,
                PostImage = post.PostImage,
                AuthorId = "",
                DisplayAuthorId = "",
                StatusName = post.StatusName,
                DateCreated = post.DateCreated,
                ModifiedDate = DateTime.Now,
                ScheduleDate = post.ScheduleDate,
                ExpirationDate = (DateTime)post.ExpirationDate
            };
            var repo = BlogRespositoryFactory.GetMode();
            repo.EditBlogPost(PostToPost(post));
            return Ok();
        }

        [Route("api/add")]
        [AcceptVerbs("POST")]
        public IHttpActionResult AddBlogPost(BlogPostInfo vm) {
            CreatePostParameters tmp = new CreatePostParameters();
            tmp.PostType = "Blog";
            tmp.Title = vm.Title;
            tmp.PostContent = vm.PostContent;
            tmp.StatusName = "Pending";
            tmp.DateCreated = vm.DateCreated;
            tmp.ScheduleDate = vm.ScheduleDate;
            tmp.ExpirationDate = vm.ExpirationDate;

            if (debug)
                tmp.AuthorId = "22222222-2222-2222-2222-222222222222";          //Sam
            else {
                //todo deduce the author id 
            }
            if (debug)
                tmp.DisplayAuthorId = "22222222-2222-2222-2222-222222222222";   //Sam
            else {
                //todo deduce the disp author id later
            }

            tmp.Tags = new List<string>();
            string str = "";
            bool flag = false;                      //clean the tags, loop de loops
            string[] delims = new string[] { "#", "^", "|", ",", ".", "/", "\\", ":", "*", "&", "%", "$", "!" };
            if (vm.Tags.Count() != 1) {
                foreach (string tag in vm.Tags) {
                    flag = false;
                    str = "";
                    foreach (char c in tag) {
                        foreach (string d in delims)
                            if (c.ToString() == d) flag = true;
                        if (!flag)
                            str += c.ToString();
                        else {
                            if (str.Length != 0) {
                                tmp.Tags.Add(str);
                                str = "";
                            }
                            flag = false;
                        }
                    }
                    if (str != "") tmp.Tags.Add(str);
                }
            } else
                tmp.Tags.Add(vm.Tags[0]);
            for (int i = 0; i < vm.Tags.Count(); i++) {     //don't even trip dawg, getting some nulls added some strip them
                if (vm.Tags[i] == null)
                    vm.Tags.RemoveAt(i);
            }
            tmp.PostImage = vm.PostImage;
            var repo = BlogRespositoryFactory.GetMode();
            int nid = repo.CreateBlogPost(tmp);
            BlogPostData final = repo.GetPostById(nid);

            return Ok("did the thing");
        }

        [Route("api/{id}/comments")]
        [AcceptVerbs("GET")]
        public IHttpActionResult Comments(int id) {
            var repo = BlogRespositoryFactory.GetMode();
            return Ok(repo.GetCommentsByPostId(id));
        }

        [Route("api/{id}/comments")]
        [AcceptVerbs("POST")]
        public IHttpActionResult NewComment(AddCommentParameters comment)
        {
            var repo = BlogRespositoryFactory.GetMode();
            return Ok(repo.NewPostComment(comment));
        }

        [Route("api/deletecomment/{commentId}")]
        [AcceptVerbs("POST")]
        public IHttpActionResult DeleteComment(int commentId)
        {
            var repo = BlogRespositoryFactory.GetMode();
            repo.DeleteCommentById(commentId);
            return Ok();
        }


        [Route("api/pagecount")]
        [AcceptVerbs("POST")]
        public IHttpActionResult CountofPosts()
        {
            return Ok(BlogRespositoryFactory.GetMode().GetApprovedPostsPageCount());
        }

        [Route("api/imageupload/{id}")]
        [AcceptVerbs("POST")]
        public async Task<HttpResponseMessage> UploadFile(string id)
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                return Request.CreateErrorResponse(HttpStatusCode.UnsupportedMediaType,
                    "The request doesn't contain valid content!");
            }

            try
            {
                var provider = new MultipartMemoryStreamProvider();
                await Request.Content.ReadAsMultipartAsync(provider);
                foreach (var file in provider.Contents)
                {

                    var dataStream = await file.ReadAsStreamAsync();
                    // use the data stream to persist the data to the server (file system etc)
                    Image image = Image.FromStream(dataStream);
                    Dictionary<string, IEnumerable<string>> headr = Request.Headers.ToDictionary(a => a.Key, a => a.Value);
                    string fname = headr["filename"].FirstOrDefault();
                    String filePath = HostingEnvironment.MapPath("~/Images/") + fname;
                    image.Save(filePath );
                    var response = Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StringContent(fname, Encoding.UTF8, "text/plain");
                    response.Content.Headers.ContentType = new MediaTypeWithQualityHeaderValue(@"text/html");
                    return response;
                }
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
            }
            return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Logic should not reach this point");
        }

        [Route("api/saveimage")]
        [AcceptVerbs("POST")]
        public IHttpActionResult SaveImage([FromBody] ImageSave parms)
        {
            
            try
            {
                String oldfilePath = HostingEnvironment.MapPath("~/Images/") + parms.OldFile;
                String newfilePath = HostingEnvironment.MapPath("~/Images/") + parms.NewFile;
                File.Move(oldfilePath, newfilePath);  
                }
            catch (Exception)
            {
                return InternalServerError();
            }
            return Ok();
        }

        [Route("api/deleteimage")]
        [AcceptVerbs("POST")]
        public IHttpActionResult DeleteImage([FromBody] ImageSave parms)
        {

            try
            {
                String oldfilePath = HostingEnvironment.MapPath("~/Images/") + parms.OldFile;
                 
                File.Delete(oldfilePath);
            }
            catch (Exception)
            {
                return InternalServerError();
            }
            return Ok();
        }

        [Route("api/blogpost/")]
        [AcceptVerbs("GET")]
        public IHttpActionResult GetBlogPost(int id)
        {
            var repository = BlogRespositoryFactory.GetMode();
            var model = repository.GetPostBodyById(id);

            return Ok(model);
        }
    }
}