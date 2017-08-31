using GhostPhotographerBlog.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GhostPhotographerBlog.UI.Controllers
{
    public class PostController : Controller
    {
        [AcceptVerbs("Get")]
        public ActionResult PostSnippet(BlogPostViewModel vm)
        {

            return PartialView("_BlogSnippet", vm);
        }
    }
}