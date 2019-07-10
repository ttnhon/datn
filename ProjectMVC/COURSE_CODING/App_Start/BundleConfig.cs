using System.Web;
using System.Web.Optimization;

namespace COURSE_CODING
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery.unobtrusive-ajax.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"
                     ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"
                       ));

            bundles.Add(new StyleBundle("~/Content/cssLogin").Include(
                      "~/Assets/Page/vendor/bootstrap/css/bootstrap.min.css",
                     "~/Assets/Page/dist/css/sb-admin-2.css"));

            bundles.Add(new StyleBundle("~/Content/cssRegister").Include(
                   "~/Assets/Page/dist/css/sb-admin.css",
                   "~/Assets/Page/vendor/bootstrap/css/bootstrap.min1.css"
                  ));
        }
    }
}
