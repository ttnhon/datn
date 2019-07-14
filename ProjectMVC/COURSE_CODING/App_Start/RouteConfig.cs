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

            routes.MapMvcAttributeRoutes();

            routes.MapRoute(
            name: "Register",
            url: "register",
            defaults: new { controller = "Authen", action = "Register", id = UrlParameter.Optional },
            namespaces: new[] { "COURSE_CODING.Controllers" }
           );

            routes.MapRoute(
               name: "LogIn",
               url: "login",
               defaults: new { controller = "Authen", action = "Login", id = UrlParameter.Optional },
               namespaces: new[] { "COURSE_CODING.Controllers" }
                  );


            routes.MapRoute(
            name: "ForgotPassword",
            url: "ForgotPassword",
            defaults: new { controller = "Authen", action = "ForgotPassword", id = UrlParameter.Optional },
            namespaces: new[] { "COURSE_CODING.Controllers" }
               );

            routes.MapRoute(
          name: "ChangePassword",
          url: "ChangePassword",
          defaults: new { controller = "Authen", action = "ChangePassword", id = UrlParameter.Optional },
          namespaces: new[] { "COURSE_CODING.Controllers" }
             );

            
              routes.MapRoute(
          name: "SignIn",
          url: "Authen/SignIn",
          defaults: new { controller = "Authen", action = "SignIn", id = UrlParameter.Optional ,type="Google"},
          namespaces: new[] { "COURSE_CODING.Controllers" }
             );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Authen", action = "Home", id = UrlParameter.Optional },
                 namespaces: new[] { "COURSE_CODING.Controllers" }
            );

        }
    }
}
