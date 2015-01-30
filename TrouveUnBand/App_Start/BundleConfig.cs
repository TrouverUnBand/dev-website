using System.Web;
using System.Web.Optimization;

namespace TrouveUnBand
{
    public class BundleConfig
    {
        // Pour plus d’informations sur le Bundling, accédez à l’adresse http://go.microsoft.com/fwlink/?LinkId=254725 (en anglais)
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.IgnoreList.Clear();

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-1.11.1.min.js",
                "~/Scripts/jquery-1.11.2-ui.min.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/bootstrapjs").Include(
                "~/Scripts/bootstrap.min.js",
                "~/Scripts/bootstrap-validation.js"));


            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.unobtrusive-ajax.js",
                "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/customjs").Include(
                "~/Scripts/homepage.js",
                "~/Scripts/searchpage.js",
                "~/Scripts/jquery-ui-timepicker-addon.js",
                "~/Scripts/alert.js",
                "~/Scripts/create-group.js",
                "~/Scripts/star-rating.js",
                "~/Scripts/ModifProfil.js",
                "~/Scripts/jquery.Jcrop.js",
                "~/Scripts/ImageUpload.js",
                "~/Scripts/ProfilePage.js",
                "~/Scripts/PictureDialog.js",
                "~/Scripts/TopSecret.js",
                "~/Scripts/bootstrap-select.js",
                "~/Scripts/back-to-top.js",
                "~/Scripts/tooltips.js",
                "~/Scripts/dropdown-fix.js",
                "~/Scripts/over-bootstrap.js",
                "~/Scripts/jquery.dotdotdot.min.js"
                ));

            // Utilisez la version de développement de Modernizr pour développer et apprendre. Puis, lorsque vous êtes
            // prêt pour la production, utilisez l’outil de génération sur http://modernizr.com pour sélectionner uniquement les tests dont vous avez besoin.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/Site.css",
                 "~/Content/bootstrap-select.css",
                 "~/Content/profile-page.css",
                 "~/Content/over-bootstrap.css",
                 "~/Content/jquery.Jcrop.css",
                 "~/Content/timepicker.css",
                 "~/Content/homepage.css",
                 "~/Content/event.css",
                 "~/Content/alerts.css",
                 "~/Content/profile-modification.css"
                ));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                "~/Content/themes/base/jquery.ui.core.css",
                "~/Content/themes/base/jquery.ui.resizable.css",
                "~/Content/themes/base/jquery.ui.selectable.css",
                "~/Content/themes/base/jquery.ui.accordion.css",
                "~/Content/themes/base/jquery.ui.autocomplete.css",
                "~/Content/themes/base/jquery.ui.button.css",
                "~/Content/themes/base/jquery.ui.dialog.css",
                "~/Content/themes/base/jquery.ui.slider.css",
                "~/Content/themes/base/jquery.ui.tabs.css",
                "~/Content/themes/base/jquery.ui.datepicker.css",
                "~/Content/themes/base/jquery.ui.progressbar.css",
                "~/Content/themes/base/jquery.ui.theme.css"));

            bundles.Add(new StyleBundle("~/Content/bootstrapcss").Include(
                "~/Content/bootstrap.css",
                "~/Content/bootstrap-responsive.min.css"));

            /*  Normalement ils sont chargé sur le cloud, mais en les chargeant 
                localement on améliore grandement la vitesse de chargement de la page.
                De plus le SDK de Facebook se charge une fois sur deux sur le cloud*/
            bundles.Add(new ScriptBundle("~/bundles/SocialMedia").Include(
                "~/Scripts/SDKs/Twitter.js",
                "~/Scripts/SDKs/Facebook.js",
                "~/Scripts/SDKs/GooglePlus.js"));
        }
    }
}
