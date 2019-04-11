using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace COURSE_CODING
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{*botdetect}",
      new { botdetect = @"(.*)BotDetectCaptcha\.ashx" });

            routes.MapRoute(
            name: "Register member",
            url: "register",
            defaults: new { controller = "User", action = "Register", id = UrlParameter.Optional },
            namespaces: new[] { "COURSE_CODING.Controllers" }
           );

            routes.MapRoute(
               name: "LogIn",
               url: "login",
               defaults: new { controller = "User", action = "Login", id = UrlParameter.Optional },
               namespaces: new[] { "COURSE_CODING.Controllers" }
                  );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
