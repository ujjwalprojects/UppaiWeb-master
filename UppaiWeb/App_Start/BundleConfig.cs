using System.Web;
using System.Web.Optimization;

namespace UppaiWeb
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrapjs").Include(
                "~/Scripts/bootstrap.min.js",
                "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/bundles/css").Include(
                      "~/Content/bootstrap.css"
                      ));
            bundles.Add(new StyleBundle("~/bundles/Admincss").Include(
                     "~/Content/fontawesome-free/css/all.css",
                     "~/Content/AdminStyle/sb-admin-2.css",
                     "~/Content/AdminStyle/Site.css",
                     "~/Content/AdminStyle/toastr.min.css",
                     "~/Content/AdminStyle/FormStyle.css"));

            bundles.Add(new ScriptBundle("~/bundles/Adminjs").Include(
                     "~/Scripts/moment.min.js",
                     "~/Scripts/jquery.easing.min.js",
                     "~/Scripts/js/sb-admin-2.js",
                     "~/Scripts/member.js",
                     "~/Scripts/toastr.js"));



            //This below  not Using in Uppain
            bundles.Add(new StyleBundle("~/bundles/templateCss").Include(
                 "~/Content/bootstrap.min.css",
                 "~/Content/template.css",
                 "~/Content/toastr.min.css",
                 "~/Content/ns-pager-style.css",
                 "~/Content/font-awesome.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/templateJs").Include(
                      "~/Scripts/off-canvas.js",
                      "~/Scripts/toastr.min.js",
                      "~/Scripts/admin.js"));

            bundles.Add(new StyleBundle("~/bundles/maincss").Include(
                       "~/Content/bootstrap.css",
                      "~/Content/font-awesome.min.css",
                      "~/Content/MainCss/flaticon/flaticon.css",
                      "~/Content/MainCss/jquery.animatedheadline.css",
                      "~/Content/MainCss/magnific-popup.css",
                      "~/Content/MainCss/owl.carousel.min.css",
                      "~/Content/MainCss/owl.theme.default.min.css",
                      "~/Content/MainCss/animate.min.css",
                      "~/Content/MainCss/jquery.datepicker.css",
                      //"~/Content/MainCss/nice-select.css",
                      "~/Content/MainCss/slicknav.min.css",
                      "~/Content/MainCss/style.css",
                      "~/Content/MainCss/responsive.css"));

            bundles.Add(new ScriptBundle("~/bundles/mainJs").Include(
                "~/Scripts/Main/main.js",
                "~/Scripts/Main/popper.min.js",
                "~/Scripts/Main/owl.carousel.min.js",
                "~/Scripts/Main/jquery.animatedheadline.min.js",
                "~/Scripts/Main/jquery.slicknav.min.js",
                "~/Scripts/Main/jquery.magnific-popup.min.js",
                "~/Scripts/Main/jquery.datepicker.min.js",
                //"~/Scripts/Main/jquery.nice-select.min.js",
                "~/Scripts/Main/waypoints-min.js"
                ));
        }
    }
}
