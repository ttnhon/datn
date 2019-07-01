using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
[assembly: OwinStartup(typeof(WebAPI.ValidateMiddleware))]

namespace WebAPI
{
    public class ValidateMiddleware
    {
        public void Configuration(IAppBuilder app)
        {
            //app.Run(context =>
            //{
            //    context.Response.ContentType = "text/plain";
            //    return context.Response.WriteAsync("Hello, world.");
            //});

            // Custom Middleare
            app.Use(typeof(JWTMiddleware));

            app.Use(typeof(ValidateCodeMiddleware));

            
        }
    }
}