using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BookWorm
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Language",
                "Books/Language/{languages}",
                new {controller = "Books", action = "Language"}
                );

            routes.MapRoute(
                "Books",
                "Books",
                new { controller = "Books", action = "List" }
                );

            routes.MapRoute(
                "Book Detail",
                "Books/{id}/{title}",
                new { controller = "Books", action = "Details", title = UrlParameter.Optional },
                new {id = @"\d+"}
                );

            routes.MapRoute(
                "News",
                "News",
                new { controller = "Posts", action = "News" }
                );

            routes.MapRoute(
                "News Details",
                "News/{id}/{title}",
                new { controller = "Posts", action = "Details", title = UrlParameter.Optional },
                new { id = @"\d+" }
                );

            routes.MapRoute(
                "Pages",
                "Pages",
                new { controller = "Pages", action = "List" }
                );

            routes.MapRoute(
                "Page Details",
                "Pages/{id}/{title}",
                new { controller = "Pages", action = "Details", title = UrlParameter.Optional },
                new { id = @"\d+" }
                );

            routes.MapRoute(
                "Users",
                "Users",
                new { controller = "Account", action = "List" }
                );

            routes.MapRoute(
                "UsersCreate",
                "Users/Create",
                new { controller = "Account", action = "Create" }
                );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}