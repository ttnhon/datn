using Microsoft.Owin;
using System;
using System.Threading.Tasks;

namespace WebAPI
{
    internal class JWTMiddleware : OwinMiddleware
    {
        public JWTMiddleware(OwinMiddleware next)
            : base(next)
        {
        }

        public async override Task Invoke(IOwinContext context)
        {
            var request = context.Request;
            var headers = request.Headers;
            if (!headers.ContainsKey("Authorization"))
            {
                context.Response.StatusCode = 400;
                context.Response.Write("Bad Request: Can't access resource");
                return;
            }

            string Token = headers["Authorization"];
            if (this.Validate(Token))
            {
                await Next.Invoke(context);
            }
            else
            {
                context.Response.StatusCode = 400;
                context.Response.Write("Bad Request: Can't access resource");
                return;
            }
        }

        protected bool Validate(string Token)
        {
            int unixTimestamp = (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            string jwtDecode = Helpers.Encrypt.DecryptString(Token, Common.Constant.SECRET_KEY_TOKEN);
            int expiredTime = Int32.Parse(jwtDecode);
            if (unixTimestamp > expiredTime)
            {
                return false;
            }
            return true;
        }
    }
}