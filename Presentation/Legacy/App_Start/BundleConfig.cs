using System.Web.Optimization;

namespace Presentation.Legacy
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                        "~/Scripts/knockout-{version}.js",
                        "~/Scripts/app.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-ui.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery-ajax").Include(
                        "~/Scripts/jquery.unobtrusive-ajax.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.min.js",
                      "~/Scripts/navbar.min.js",
                      "~/Scripts/respond.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/authentication").Include(
                      "~/Scripts/Authentication/jquery.backstretch.min.js",
                      "~/Scripts/Authentication/scripts.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/challenge-create").Include(
                     "~/Scripts/Challenges/dynamic-fields.min.js",
                     "~/Scripts/ckeditor/ckeditor.js"));

            bundles.Add(new ScriptBundle("~/bundles/challenge-index").Include(
                    "~/Scripts/Challenges/searchbar.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/challenge-solve").Include(
                    "~/Scripts/star-rating.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/star-rating").Include(
                    "~/Scripts/star-rating.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/search").Include(
                     "~/Scripts/search.min.js",
                     "~/Scripts/autocomplete.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/tagcloud").Include(
                    "~/Scripts/jquery.tagcanvas.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/signalr").Include(
                "~/Scripts/jquery.signalR-2.2.0.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/user-profile").Include(
                    "~/Scripts/bootstrap-editable.min.js",
                    "~/Scripts/jquery.knob.js"));

            // scripts above, styles below

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/navbar.css",
                      "~/Content/site.css",
                      "~/Content/scrollbars.min.css",
                      "~/Content/jquery-ui.css"));

            bundles.Add(new StyleBundle("~/Content/light").Include(
                      "~/Content/Themes/bootstrap.light.min.css"));

            bundles.Add(new StyleBundle("~/Content/dark").Include(
                      "~/Content/Themes/hamburger.dark.css",
                      "~/Content/Themes/bootstrap.dark.min.css"));

            bundles.Add(new StyleBundle("~/Content/authentication").Include(
                      "~/Content/Authentication/font-roboto.css",
                      "~/Content/Authentication/font-awesome.min.css",
                      "~/Content/Authentication/form-elements.css",
                      "~/Content/bootstrap.css",
                      "~/Content/Authentication/style.css"));

            bundles.Add(new StyleBundle("~/Content/bootstrap-markdown").Include(
                      "~/Content/bootstrap-markdown/bootstrap-markdown.min.css"));

            bundles.Add(new StyleBundle("~/Content/challenge-create").Include(
                      "~/Scripts/dropzone/basic.min.css",
                      "~/Scripts/dropzone/dropzone.min.css",
                      "~/Content/Challenges/create.min.css"));

            bundles.Add(new StyleBundle("~/Content/search").Include(
                      "~/Content/search.min.css",
                      "~/Content/autocompletion.min.css"));

            bundles.Add(new StyleBundle("~/Content/challenge-index").Include(
                      "~/Content/Challenges/index.min.css"));

            bundles.Add(new StyleBundle("~/Content/challenge-solve").Include(
                      "~/Content/Challenges/solve.min.css"));

            bundles.Add(new StyleBundle("~/Content/home-index").Include(
                      "~/Content/Home/index.min.css"));

            bundles.Add(new StyleBundle("~/Content/achievement").Include(
                     "~/Content/Achievements/style.min.css"));

            bundles.Add(new StyleBundle("~/Content/fonts").Include(
                "~/Content/Fonts/titillium.css",
                "~/Content/Fonts/open-sans.css"));

            bundles.Add(new StyleBundle("~/Content/user-profile").Include(
                      "~/Content/bootstrap-editable.css"));

            bundles.IgnoreList.Clear();
        }
    }
}
