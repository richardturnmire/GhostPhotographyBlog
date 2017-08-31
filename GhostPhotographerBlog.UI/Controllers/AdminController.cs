using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GhostPhotographerBlog.Data.Interfaces;
using GhostPhotographerBlog.Data.Factory;
using GhostPhotographerBlog.Data.Repositories;
using GhostPhotographerBlog.UI.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace GhostPhotographerBlog.UI.Controllers
{
    public class AdminController : Controller
    {


        IBlogRepository repo = BlogRespositoryFactory.GetMode();
    

        [Route("admin/")]
        public ActionResult Index() {
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

            AdminPageModel tmp = new AdminPageModel();
            tmp.Posts = repo.GetPendingPosts();
            tmp.Users = repo.GetAllUsers();
            return View(tmp);
        }
        // GET: Admin
        [Route("admin/edit/{id}")]
       public ActionResult Edit(int id) {
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

            return View("Edit", repo.GetPostById(id));
        }

        [Route("admin/new")]
        [AcceptVerbs("GET")]
        public ActionResult New() {
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

            return View("New", repo.GetAllUsers());
        }

        // POST: Admin/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
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

            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Delete/5
        public ActionResult Delete(int id)
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

            return View();
        }

        // POST: Admin/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
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

            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
