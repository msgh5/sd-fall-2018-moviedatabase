using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MovieDatabase
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //Enables MVC to create routes from attributes in the action
            routes.MapMvcAttributeRoutes();

            //Another way of handling custom routes
            //routes.MapRoute(
            //    name: "MyMovieRoute",
            //    url: "mymovies/{name}",
            //    defaults: new { controller = "Movie", action = "DetailsByName" }
            //);

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
