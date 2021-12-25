using System.Web;
using System.Web.Optimization;

namespace SEAdd
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*" ,
                        "~/Scripts/jquery.unobtrusive*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/style.css"
                      ));
            bundles.Add(new StyleBundle("~/bundles/DataTables/css").Include(
                      "~/assets/DataTables-1.11.3/css/dataTables.bootstrap5.css", 
                      "~/assets/datatables.min.css"
                      ));
            bundles.Add(new ScriptBundle("~/bundles/DataTables/js").Include(
                      "~/assets/DataTables-1.11.3/js/jquery.dataTables.min.js", 
                      "~/assets/DataTables-1.11.3/js/dataTables.bootstrap5.js"
                      ));
        }
    }
}
