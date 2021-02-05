using System.Web;
using System.Web.Optimization;

namespace RIS
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                    
                      "~/Content/assets/css/material-dashboard.min40a0.css?v=2.0.2",
                      "~/Content/assets/demo/demo.css"));

            bundles.Add(new ScriptBundle("~/Content/js").Include(
                "~/Content/assets/js/core/jquery.min.js",
                "~/Content/assets/js/core/popper.min.js",
                "~/Content/assets/js/core/bootstrap-material-design.min.js",
                "~/Content/assets/js/plugins/perfect-scrollbar.jquery.min.js",
                "~/Content/assets/js/plugins/moment.min.js",
                "~/Content/assets/js/plugins/sweetalert2.js",
                "~/Content/assets/js/plugins/jquery.bootstrap-wizard.js",
                "~/Content/assets/js/plugins/bootstrap-selectpicker.js",
                "~/Content/assets/js/plugins/bootstrap-datetimepicker.min.js",
                "~/Content/assets/js/plugins/jquery.dataTables.min.js",
                "~/Content/assets/js/plugins/bootstrap-tagsinput.js",
                "~/Content/assets/js/plugins/jasny-bootstrap.min.js",
                "~/Content/assets/js/plugins/fullcalendar.min.js",
                "~/Content/assets/js/plugins/nouislider.min.js",
                "~/Content/assets/js/plugins/arrive.min.js",
                "~/Content/assets/js/plugins/chartist.min.js",
                "~/Content/assets/js/plugins/bootstrap-notify.js",
                "~/Scripts/Sidebar.js",
                "~/Scripts/Custom.js"

            ));
        }
    }
}
