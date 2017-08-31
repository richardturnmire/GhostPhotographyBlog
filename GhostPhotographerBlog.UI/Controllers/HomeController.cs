using GhostPhotographerBlog.Data.Factory;
using GhostPhotographerBlog.Data.Repositories;
using GhostPhotographerBlog.Models.Queries;
using GhostPhotographerBlog.UI.ActionFilters;
using GhostPhotographerBlog.UI.Helpers;
using GhostPhotographerBlog.UI.Models;
using GhostPhotographerBlog.UI.Properties;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace GhostPhotographerBlog.UI.Controllers
{
    [LogActionFilter]
    public class HomeController : Controller
    {
        BlogRepository _repo = new BlogRepository();

        private void AssignRoles()
        {
            var userMgr = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var user = userMgr.FindByName(User.Identity.Name);
            ViewBag.UserId = user.Id;
            var roles = userMgr.GetRoles(user.Id);
            ViewBag.Roles = roles;
            var role = userMgr.GetRoles(user.Id)[0];
            ViewBag.Role = role;
        }

        public HomeController()
        {
            
        }

        public ActionResult Index([Bind(Prefix = "SearchType")] int? id, string searchArg)
        {
            if (Request.IsAuthenticated)
            {
                AssignRoles();
            }
            else { }

           int searchVal =  id ?? 99;
           
           if (string.IsNullOrEmpty(searchArg)) searchArg = String.Empty;
                
                
            List<BlogPostInfo> repo = new List<BlogPostInfo>();
            var pageCount = 0;

            switch ((SearchType)searchVal)
            {
                case SearchType.All:
                   repo = BlogRespositoryFactory.GetMode().SearchPostsByAny(searchArg);
                    pageCount = repo.Count / 10 + 1;
                    break;
                case SearchType.Title:
                    repo = BlogRespositoryFactory.GetMode().SearchPostsByTitle(searchArg);
                    pageCount = repo.Count / 10 + 1;
                    break;
                case SearchType.Content:
                    repo = BlogRespositoryFactory.GetMode().SearchPostsByContent(searchArg);
                    pageCount = repo.Count / 10 + 1;
                    break;
                case SearchType.Tag:
                    repo = BlogRespositoryFactory.GetMode().SearchPostsByHashtag(searchArg);
                    pageCount = repo.Count / 10 + 1;
                    break;
                default:
                    repo = BlogRespositoryFactory.GetMode().GetPageOfApprovedPosts(1);
                    pageCount = BlogRespositoryFactory.GetMode().GetApprovedPostsPageCount();
                    Console.WriteLine(Resources.HomeController_Search_Default_case);
                    break;
            }
           
            List<BlogPostViewModel> blogs = new List<BlogPostViewModel>();

            foreach (BlogPostInfo b in repo)
            {
                string pattern = "</p>";
                string[] paragraphs = Regex.Split(b.PostContent, @pattern);
                paragraphs[0] += "</p>";
                b.PostContent = paragraphs[0];

                blogs.Add(new BlogPostViewModel(b));
            }

            ViewBag.JumboImage = "../../Images/Jumbotron-1.jpg";

            return View(new BlogPostSnippetViewModel()
            {
                Snippets = blogs,
                HashTags = GetTagsAndWeights(),
                BlogPostPageCount = pageCount
            });
        }

    
        public ActionResult NewPage(int id)
        {
            if (Request.IsAuthenticated)
            {
                AssignRoles();
            }
            else { }

            var repo = BlogRespositoryFactory.GetMode().GetPageOfApprovedPosts(id);
            var pageCount = BlogRespositoryFactory.GetMode().GetApprovedPostsPageCount();

            List<BlogPostViewModel> blogs = new List<BlogPostViewModel>();

            foreach (BlogPostInfo b in repo)
            {
                string pattern = "</p>";
                string[] paragraphs = Regex.Split(b.PostContent, @pattern);
                paragraphs[0] += "</p>";
                b.PostContent = paragraphs[0];

                blogs.Add(new BlogPostViewModel(b));
            }

            ViewBag.JumboImage = "../../Images/Jumbotron-1.jpg";

            return View("Index", new BlogPostSnippetViewModel()
            {
                Snippets = blogs,
                HashTags = GetTagsAndWeights(),
                BlogPostPageCount = pageCount
            });
        }
        public IEnumerable<HashTagWeightViewModel> GetTagsAndWeights()
        {
            
            var hashtags = BlogRespositoryFactory.GetMode().GetHashtagInfo();
            List<HashTagWeightViewModel> hashWeight = new List<HashTagWeightViewModel>();
            var maxCount = hashtags.Max(h => h.Count);

            foreach (var hashtag in hashtags)
            {
                var decCount = (decimal)hashtag.Count;
                decimal wgt = Math.Round(decCount / maxCount * 10m);
                if (wgt < 1m) wgt = 1m;
                var hashtagVm = new HashTagWeightViewModel()
                {
                    HashtagName = hashtag.HashtagName,
                    Count = hashtag.Count,

                    HashtagWeight = (int)wgt
                };
                hashWeight.Add(hashtagVm);
            }

            return hashWeight;
        }
        public ActionResult About()
        {
            if (Request.IsAuthenticated)
            {
                AssignRoles();
            }
            else { }

            ViewBag.Message = "About Me...";

            return View();
        }

        public ActionResult Contact()
        {
            if (Request.IsAuthenticated)
            {
                AssignRoles();
            }
            else { }

            ViewBag.Message = "My favorites";

            return View();
        }

        public ActionResult FileUpload()
        {
            if (Request.IsAuthenticated)
            {
                AssignRoles();
            }
            else { }

            ViewBag.Message = "My favorites";

            return View();
        }

        public ActionResult Blog(int id)
        {
            if (Request.IsAuthenticated)
            {
                // This is how you get the User GUID
                var userMgr = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                var user = userMgr.FindByName(User.Identity.Name);

                // Then put the User GUID (i.e. AspNetUsers Id) into the ViewBag.
                // Note - ViewBag is a dynamic object... you make up any property you want (e.g. .UserId).
                ViewBag.UserId = user.Id;

                // Get list of roles
                var roles = userMgr.GetRoles(user.Id);
                ViewBag.Roles = roles;

                // Get the role if there is only 1
                var role = userMgr.GetRoles(user.Id)[0];
                ViewBag.Role = role;
            }
            else
            {
                // No one logged in (Anonymous User).  The view will not show content other than a message.
            }

            var repo = BlogRespositoryFactory.GetMode().GetApprovedPostById(id);
            BlogPostViewModel model = new BlogPostViewModel(repo);

            return View("BlogPost", model);
        }

        [System.Web.Http.AcceptVerbs("Get")]
        public ActionResult Comment(int Id, int PostId, string Comment, DateTime CommentDate, string UserName, string DisplayName)
        {
            if (Request.IsAuthenticated)
            {
                AssignRoles();
            }
            else { }

            Comments Commented = new Comments
            {
                Id = Id,
                PostId = PostId,
                Comment = Comment,
                CommentDate = CommentDate,
                UserName = UserName,
                DisplayName = DisplayName
            };

            CommentViewModel commentVM = new CommentViewModel(Commented);

            return PartialView("_Comment", commentVM);
        }

       
      
    }
}