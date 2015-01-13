using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace AllocatorShare2
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Auth",
                url: "auth",
                defaults: new { controller = "Home", action = "Auth", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Upload",
                url: "upload/{id}",
                defaults: new { controller = "Home", action = "Upload", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
