using System.Diagnostics.CodeAnalysis;
using System.Web.Optimization;

namespace BikeTracker
{
    [ExcludeFromCodeCoverage]
    public static class BundleConfig
    {
        private readonly static string AngularCDN = "//ajax.googleapis.com/ajax/libs/angularjs/1.2.29/angular.min.js";
        private readonly static string AngularRouteCDN = "//ajax.googleapis.com/ajax/libs/angularjs/1.2.29/angular-route.min.js";
        private readonly static string AngularResourceCDN = "//ajax.googleapis.com/ajax/libs/angularjs/1.2.29/angular-resource.min.js";
        private readonly static string AngularBootstrapCDN = "//cdnjs.cloudflare.com/ajax/libs/angular-ui-bootstrap/0.12.0/ui-bootstrap.min.js";
        private readonly static string AngularBootstrapTemplatesCDN = "//cdnjs.cloudflare.com/ajax/libs/angular-ui-bootstrap/0.12.0/ui-bootstrap-tpls.min.js";

        private readonly static string JQueryCDN = "//ajax.googleapis.com/ajax/libs/jquery/1.12.2/jquery.min.js";

        private readonly static string BootstrapCDN = "//maxcdn.bootstrapcdn.com/bootstrap/3.3.6/js/bootstrap.min.js";

        private static void RegisterAngularModule(this BundleCollection bundles, string bundleName, string moduleName, string cdn, string local)
        {
            var angularBundle = new ScriptBundle($"~/bundles/{bundleName}", cdn).Include(local);
            angularBundle.CdnFallbackExpression = $"(function() {{ try {{ window.angular.module('{moduleName}'); }} catch (e) {{ return false; }} return true; }})()";

            bundles.Add(angularBundle);
        }

        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.UseCdn = true;

            var angularBundle = new ScriptBundle("~/bundles/angular", AngularCDN).Include("~/lib/angular/angular.js");
            angularBundle.CdnFallbackExpression = "window.angular";
            bundles.Add(angularBundle);

            var i18nBundle = new ScriptBundle("~/bundles/angular-i18n").Include("~/lib/angular-i18n/angular-locale_en-gb.js");
            bundles.Add(i18nBundle);

            bundles.RegisterAngularModule("angular-route", "ngRoute", AngularRouteCDN, "~/lib/angular-route/angular-route.js");
            bundles.RegisterAngularModule("angular-resource", "ngResource", AngularResourceCDN, "~/lib/angular-resource/angular-resource.js");
            bundles.RegisterAngularModule("angular-bootstrap", "ui.bootstrap", AngularBootstrapCDN, "~/lib/angular-bootstrap/ui-bootstrap.js");
            bundles.RegisterAngularModule("angular-bootstrap-templates", "ui.bootstrap.tpls", AngularBootstrapTemplatesCDN, "~/lib/angular-bootstrap/ui-bootstrap-tpls.js");

            bundles.Add(new ScriptBundle("~/bundles/angular-chart").Include("~/lib/Chart.js/Chart.js")
                .Include("~/lib/angular-chart.js/angular-chart.js"));

            bundles.Add(new ScriptBundle("~/bundles/site-core")
                .Include("~/Scripts/ui/app.js")
                .Include("~/Scripts/ui/controllers.js")
                .Include("~/Scripts/ui/services.js"));

            var jqueryBundle = new ScriptBundle("~/bundles/jquery", JQueryCDN)
                .Include("~/lib/jquery/dist/jquery.js");
            jqueryBundle.CdnFallbackExpression = "window.jQuery";
            bundles.Add(jqueryBundle);

            var bootstrapBundle = new ScriptBundle("~/bundles/bootstrap", BootstrapCDN)
                .Include("~/lib/bootstrap/dist/js/bootstrap.js");
            bootstrapBundle.CdnFallbackExpression = "$.fn.modal";
            bundles.Add(bootstrapBundle);

            var styleBundle = new StyleBundle("~/bundles/css").Include("~/Content/theme.css")
                .Include("~/lib/angular-chart.js/angular-chart.css");
            bundles.Add(styleBundle);
        }
    }
}