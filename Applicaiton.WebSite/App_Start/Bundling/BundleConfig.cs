using Application.WebSite.App_Start.Bundling;
using System.Web.Optimization;

namespace Application.WebSite
{
    public class BundleConfig
    {
        // 有关绑定的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.IgnoreList.Clear();

            AddStyleSheets(bundles);
            AddScripts(bundles);

            BundleTable.EnableOptimizations = false;
        }

        private static void AddStyleSheets(BundleCollection bundles)
        {
            bundles.Add(
                new StyleBundle("~/Bundles/WebFrame.css")
                .Include("~/Content/WebFrame/css/global/font-awesome.css", new CssRewriteUrlWithVirtualDirectoryTransform())
                .Include("~/Content/WebFrame/css/global/common.css", new CssRewriteUrlWithVirtualDirectoryTransform())
                .IncludeDirectory("~/Content/WebFrame/css/base/", "*.css", true)
                .IncludeDirectory("~/Content/WebFrame/css/plugin/", "*.css", true)
                .IncludeDirectory("~/Content/WebFrame/css/component/", "*.css", true)
                .IncludeDirectory("~/Content/WebFrame/css/module/page/", "*.css", true).ForceOrdered()
                .IncludeDirectory("~/Content/WebFrame/css/module/common/", "*.css", true)

                .Include("~/Content/WebFrame/js/external/jstree/themes/default/style.css", new CssRewriteUrlWithVirtualDirectoryTransform())
                );
        }

        private static void AddScripts(BundleCollection bundles)
        {
            bundles.Add(
                new ScriptBundle("~/Bundles/WebFrame.js").Include(
                    //global
                    "~/Content/WebFrame/js/global/jquery/jquery-2.1.1.js",
                    "~/Content/WebFrame/js/global/common.js",

                    //external
                    "~/Content/WebFrame/js/external/underscore.js",
                    "~/Content/WebFrame/js/external/moment.js",
                    "~/Content/WebFrame/js/external/jstree/jstree.js",
                    "~/Content/WebFrame/js/external/signalR/jquery.signalR-2.2.1.js",
                    "~/Content/WebFrame/js/external/angularJS/angular.min.js",
                    "~/Content/WebFrame/js/external/angularJS/angular-ui-router.js",
                    "~/Content/WebFrame/js/external/angularJS/ui/ui-utils.js"
                    )
                    //lib
                    .IncludeDirectory(
                    "~/Content/WebFrame/js/lib/", "*.js", false).ForceOrdered()
                    //plugin
                    .IncludeDirectory(
                    "~/Content/WebFrame/js/plugin/", "*.js", true).ForceOrdered()
                );
            bundles.Add(
                new ScriptBundle("~/Bundles/WebFrame/angular.js")
                .IncludeDirectory("~/Content/WebFrame/js/angular/", "*.js", true).ForceOrdered()
                );

            bundles.Add(
               new ScriptBundle("~/Bundles/Infrastructure.js")
                   .IncludeDirectory("~/Script/Frame/", "*.js", true).ForceOrdered()
               );

            bundles.Add(
               new ScriptBundle("~/Bundles/Common.js")
                   .IncludeDirectory("~/Script/Common/", "*.js", true).ForceOrdered()
               );

            bundles.Add(
                new ScriptBundle("~/Bundles/App/Common.js")
                    .IncludeDirectory("~/Script/App/Common/", "*.js", false)
                    .IncludeDirectory("~/Script/App/Common/services", "*.js", true)
                    .IncludeDirectory("~/Script/App/Common/directives", "*.js", true)
                    .IncludeDirectory("~/Script/App/Common/filters", "*.js", true).ForceOrdered()
                );

            bundles.Add(
                new ScriptBundle("~/Bundles/App/Manager.js")
                    .IncludeDirectory("~/Script/App/Manager/", "*.js", true).ForceOrdered()
                );
        }
    }
}
