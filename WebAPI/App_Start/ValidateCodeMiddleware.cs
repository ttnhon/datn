using Microsoft.Owin;
using System;
using System.IO;
using System.Threading.Tasks;
using WebAPI.Models;
namespace WebAPI
{
    internal class ValidateCodeMiddleware : OwinMiddleware
    {
        public ValidateCodeMiddleware(OwinMiddleware next)
            : base(next)
        {
        }

        public async override Task Invoke(IOwinContext context)
        {
            //context.Response.Headers["MachineName"] = Environment.MachineName;
            var requestBody = new StreamReader(context.Request.Body).ReadToEnd();
            string Code = context.Request.Get<string>("Code");
            string Language = context.Request.Get<string>("Language");
            if (this.Validate(Code, Language))
            {
                await Next.Invoke(context);
            }
            else
            {
                context.Response.StatusCode = 400;
                context.Response.Write("Bad Request: Block the injection code");
                return;
            }
            
            
        }

        protected bool Validate(string Code, string Language)
        {
            switch (Language)
            {
                case "CPP":
                    return this.ValidateCpp(Code);
                case "CSHARP":
                    return this.ValidateCSharp(Code);
                case "JAVA":
                    return this.ValidateJava(Code);
                default:
                    break;
            }
            return true;
        }

        protected bool ValidateCpp(string Code)
        {
            return true;
        }

        protected bool ValidateCSharp(string Code)
        {
            return true;
        }

        protected bool ValidateJava(string Code)
        {
            return true;
        }
    }
}