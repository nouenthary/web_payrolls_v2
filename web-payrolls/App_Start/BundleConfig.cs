using System.Web;
using System.Web.Optimization;

namespace web_payrolls
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //BundleTable.EnableOptimizations = true;
            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/Scripts/jquery-{version}.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/Scripts/jquery.validate*"));

            //// Use the development version of Modernizr to develop with and learn from. Then, when you're
            //// ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));    

            //bundles.Add(new StyleBundle("~/Content/css").Include(                    
            //          "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/css/animate.css",
                      "~/Content/css/spinners.css",
                      "~/Content/css/style.css",
                      "~/Content/css/colors/blue.css"                     
                      ));        

            bundles.Add(new StyleBundle("~/Content/assets").Include(
                "~/Content/assets/plugins/bootstrap/css/bootstrap.min.css",
                "~/Content/assets/plugins/chartist-js/dist/chartist.min.css",
                "~/Content/assets/plugins/chartist-js/dist/chartist-init.css",
                "~/Content/assets/plugins/chartist-plugin-tooltip-master/dist/chartist-plugin-tooltip.css",
                "~/Content/assets/plugins/c3-master/c3.min.css",
                "~/Content/assets/plugins/dropify/dist/css/dropify.min.css",
                "~/Content/assets/plugins/sweetalert/sweetalert.css",
                "~/Content/assets/plugins/bootstrap-datepicker/bootstrap-datepicker.min.css",
                "~/Content/assets/plugins/timepicker/bootstrap-timepicker.min.css",
                "~/Content/assets/plugins/daterangepicker/daterangepicker.css",
                "~/Content/assets/plugins/chartist-js/dist/chartist.min.css",
                "~/Content/assets/plugins/chartist-js/dist/chartist-init.css",
                "~/Content/assets/plugins/chartist-plugin-tooltip-master/dist/chartist-plugin-tooltip.css",
                "~/Content/assets/plugins/css-chart/css-chart.css",
                "~/Content/assets/plugins/typeahead.js-master/dist/typehead-min.css"
            ));

            bundles.Add(new StyleBundle("~/Content/custom").Include(
            "~/Content/custom/app.css"
            ));

            bundles.Add(new StyleBundle("~/Content/asset").Include(
                "~/Content/assets/plugins/jquery/jquery.min.js",
                "~/Content/assets/plugins/popper/popper.min.js",
                "~/Content/assets/plugins/bootstrap/js/bootstrap.min.js",
                "~/Content/assets/plugins/sticky-kit-master/dist/sticky-kit.min.js",
                "~/Content/assets/plugins/sparkline/jquery.sparkline.min.js",
                "~/Content/assets/plugins/chartist-js/dist/chartist.min.js",
               // "~/Content/assets/plugins/chartist-plugin-tooltip-master/dist/chartist-plugin-tooltip.min.js",
                "~/Content/assets/plugins/d3/d3.min.js",
                "~/Content/assets/plugins/c3-master/c3.min.js",
                "~/Content/assets/plugins/styleswitcher/jQuery.style.switcher.js",
                "~/Content/assets/plugins/datatables/jquery.dataTables.min.js",
                "~/Content/assets/plugins/dropify/dist/js/dropify.min.js",
                "~/Content/assets/plugins/sweetalert/jquery.sweet-alert.custom.js",
                "~/Content/assets/plugins/sweetalert/sweetalert.min.js",
                "~/Content/assets/plugins/moment/moment.js",
                "~/Content/assets/plugins/bootstrap-datepicker/bootstrap-datepicker.min.js",
                "~/Content/assets/plugins/timepicker/bootstrap-timepicker.min.js",
                "~/Content/assets/plugins/daterangepicker/daterangepicker.js",
                "~/Content/assets/plugins/typeahead.js-master/dist/typeahead.bundle.min.js"
            ));

            
            bundles.Add(new StyleBundle("~/Content/js").Include(
            "~/Content/js/jquery.slimscroll.js",
            "~/Content/js/waves.js",
            "~/Content/js/sidebarmenu.js",
            "~/Content/js/custom.min.js"
           // "~/Content/js/dashboard1.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/csvExport.js"));
        }
    }
}
