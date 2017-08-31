using GhostPhotographerBlog.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace GhostPhotographerBlog.UI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // routes.MapMvcAttributeRoutes();

            //routes.MapRoute(
            //     "Search",
            //    "Home/Search/{searchType}/{searchArg}",
            //    defaults: new { controller = "Home", action = "Search", id = UrlParameter.Optional }

            //);

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional}
            );
        }
    }
}
