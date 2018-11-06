using System.Web;
using System.Web.Optimization;

namespace Shop
{
    public class BundleConfig
    {
        // Para obtener más información sobre las uniones, visite https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            // CSS
            bundles.Add(new StyleBundle("~/css/animate").Include(
                        "~/Content/css/animate.css"));

            bundles.Add(new StyleBundle("~/css/fontawesome").Include(
                        "~/Content/css/font-awesome.min.css"
                        ));

            bundles.Add(new StyleBundle("~/css/bootstrap").Include(
                        "~/Content/css/bootstrap.min.css"));

            bundles.Add(new StyleBundle("~/css/style").Include(
                        "~/Content/css/flexslider.css",
                         "~/Content/css/owl.carousel.min.css",
                          "~/Content/css/owl.theme.default.min.css",
                          "~/Content/css/simple-sidebar.css",
                          "~/Content/css/style.css"
                        ));
            bundles.Add(new StyleBundle("~/css/sweetAlert").Include(
                        "~/Content/css/sweetalert.css"));

            // JS
            bundles.Add(new ScriptBundle("~/js/bootstrap").Include(
                        "~/Content/js/bootstrap.min.js"));

            bundles.Add(new ScriptBundle("~/js/ModernizrJs").Include(
                        "~/Content/js/modernizr-2.6.2.min.js"));

            bundles.Add(new ScriptBundle("~/js/jQuery").Include(
                        "~/Content/js/jquery.min.js",
                        "~/Content/js/jquery.easing.1.3.js",
                        "~/Content/js/jquery.waypoints.min.js",
                        "~/Content/js/jquery.countTo.js",
                        "~/Content/js/jquery.flexslider-min.js"
                        ));

            bundles.Add(new ScriptBundle("~/js/carousel").Include(
                        "~/Content/js/owl.carousel.min.js"));

            bundles.Add(new ScriptBundle("~/js/main").Include(
                        "~/Content/js/main.js"));
            bundles.Add(new ScriptBundle("~/js/sweetAlert").Include(
                       "~/Content/js/sweetalert.min.js"));

        }
    }
}
