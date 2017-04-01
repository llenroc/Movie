using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Web;

namespace Utility.Web
{
    public class PathHelper
    {
        public static string GetAbsolutePath(string path)
        {
            //return Path.Combine(HttpRuntime.AppDomainAppPath, path);
            return System.Web.Hosting.HostingEnvironment.MapPath("~"+ path);
        }
    }
}
