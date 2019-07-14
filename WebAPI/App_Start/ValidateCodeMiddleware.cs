using Microsoft.Owin;
using System;
using System.IO;
using System.Threading.Tasks;
using WebAPI.Models;
using System.Text.RegularExpressions;
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
            //string regex = @".system";
            
            //if (Regex.IsMatch(Code, regex)) return false;
            return true;
        }

        protected bool ValidateCSharp(string Code)
        {
            return true;
        }

        protected bool ValidateJava(string Code)
        {
            bool result = true;
            
            //validate exec
            string[] exec_catch = new string[]{"new ProcessBuilder(","Runtime.getRuntime().exec", };
            foreach (string catch_exec in exec_catch)
            {
                if (Code.Contains(catch_exec))
                {
                    result = false;
                }
            }

            //validate Read and Write file

            return result;
        }
    }
}