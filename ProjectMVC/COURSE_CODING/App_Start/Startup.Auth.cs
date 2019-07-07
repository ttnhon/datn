using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;

namespace COURSE_CODING
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);
            //app.UseCookieAuthentication(new CookieAuthenticationOptions
            //{
            //    LoginPath = new PathString("/Account/Index"),
            //    SlidingExpiration = true
            //});
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                LoginPath = new PathString("/Authen/Login"),
                SlidingExpiration = true
            });
            app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            {
                ClientId = "802168017479-g3c8lup4nrdb442r6ekebvdqf22jq5s6.apps.googleusercontent.com",
                ClientSecret = "T2wl53Bx3B6c9z1_PKfOgakC",
                CallbackPath = new PathString("/GoogleLoginCallback")
            });
        }
    }
}