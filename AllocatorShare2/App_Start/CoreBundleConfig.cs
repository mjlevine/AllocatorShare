using System.Web.Optimization;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(AllocatorShare2.CoreBundleConfig), "Start")]
namespace AllocatorShare2
{
    public static class CoreBundleConfig
    {
        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            RegisterBundles(BundleTable.Bundles);
        }

        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/bundles/coreStyleBundle").Include(
                "~/bower_components/bootstrap/dist/css/bootstrap.css",
                "~/Content/mk6-vantage.min.css",
                "~/bower_components/toastr/toastr.min.css"
                ));

            bundles.Add(new ScriptBundle("~/bundles/coreScriptBundle").Include(
                "~/bower_components/console-polyfill/index.js",
                "~/bower_components/es5-shim/es5-shim.js",
                "~/bower_components/es5-shim/es5-sham.js",
                "~/bower_components/jquery/jquery.min.js",
                "~/bower_components/react/JSXTransformer.js",
                "~/bower_components/react/react.min.js",
                "~/bower_components/jquery-form/jquery.form.js",
                "~/bower_components/underscore/underscore.js",
                "~/bower_components/moment/min/moment.min.js",
                "~/Scripts/Highcharts-4.0.1/js/highcharts.js",
				"~/Scripts/Highcharts-4.0.1/js/adapters/standalone-framework.js",
                "~/Scripts/mk6/eventService.js",
                "~/bower_components/toastr/toastr.js"
                ));
        }
    }
}
