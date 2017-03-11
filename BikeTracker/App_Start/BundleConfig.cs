using System.Diagnostics.CodeAnalysis;
using System.Web.Optimization;

namespace BikeTracker
{
    [ExcludeFromCodeCoverage]
    public static class BundleConfig
    {
        private const string AngularBootstrapTemplatesCDN = "https://cdnjs.cloudflare.com/ajax/libs/angular-ui-bootstrap/2.5.0/ui-bootstrap-tpls.min.js";
        private const string AngularCDN = "https://cdnjs.cloudflare.com/ajax/libs/angular.js/1.6.3/angular.min.js";
        private const string AngularResourceCDN = "https://cdnjs.cloudflare.com/ajax/libs/angular.js/1.6.3/angular-resource.min.js";
        private const string AngularRouteCDN = "https://cdnjs.cloudflare.com/ajax/libs/angular.js/1.6.3/angular-route.min.js";
        private const string BootstrapCDN = "https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.3.7/js/bootstrap.min.js";
        private const string JQueryCDN = "https://cdnjs.cloudflare.com/ajax/libs/jquery/3.1.1/jquery.min.js";

        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.UseCdn = true;

            var angularBundle = new ScriptBundle("~/bundles/angular", AngularCDN).Include("~/lib/angular/angular.min.js");
            angularBundle.CdnFallbackExpression = "window.angular";
            bundles.Add(angularBundle);

            var i18nBundle = new ScriptBundle("~/bundles/angular-i18n").Include("~/lib/angular-i18n/angular-locale_en-gb.js");
            bundles.Add(i18nBundle);

            bundles.RegisterAngularModule("angular-route", "ngRoute", AngularRouteCDN, "~/lib/angular-route/angular-route.min.js");
            bundles.RegisterAngularModule("angular-resource", "ngResource", AngularResourceCDN, "~/lib/angular-resource/angular-resource.min.js");
            bundles.RegisterAngularModule("angular-bootstrap-templates", "ui.bootstrap", AngularBootstrapTemplatesCDN, "~/lib/angular-ui-bootstrap/ui-bootstrap-tpls.js");

            bundles.Add(new ScriptBundle("~/bundles/angular-chart").Include("~/lib/chart.js/Chart.min.js")
                .Include("~/lib/angular-chart.js/angular-chart.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/site-core")
                .Include("~/js/site.js")
                .Include("~/Scripts/ui/app.js")
                .Include("~/Scripts/ui/controllers.js")
                .Include("~/Scripts/ui/services.js"));

            var jqueryBundle = new ScriptBundle("~/bundles/jquery", JQueryCDN)
                .Include("~/lib/jquery/jquery.min.js");
            jqueryBundle.CdnFallbackExpression = "window.jQuery";
            bundles.Add(jqueryBundle);

            var bootstrapBundle = new ScriptBundle("~/bundles/bootstrap", BootstrapCDN)
                .Include("~/lib/bootstrap/js/bootstrap.min.js");
            bootstrapBundle.CdnFallbackExpression = "$.fn.modal";
            bundles.Add(bootstrapBundle);

            var styleBundle = new StyleBundle("~/bundles/css").Include("~/Content/theme.css");
            bundles.Add(styleBundle);
        }

        private static void RegisterAngularModule(this BundleCollection bundles, string bundleName, string moduleName, string cdn, string local)
        {
            var angularBundle = new ScriptBundle($"~/bundles/{bundleName}", cdn).Include(local);
            angularBundle.CdnFallbackExpression = $"(function() {{ try {{ window.angular.module('{moduleName}'); }} catch (e) {{ return false; }} return true; }})()";

            bundles.Add(angularBundle);
        }
    }
}