using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Application.WebSite.Middleware
{
    public class HelloWorldMiddleware : OwinMiddleware
    {
        public HelloWorldMiddleware(OwinMiddleware next) : base(next)
        {

        }


        public override Task Invoke(IOwinContext context)
        {
            string response = "Hello World! It is" + DateTime.Now;
            context.Response.Write(response);
            return Next.Invoke(context);
        }
    }
}