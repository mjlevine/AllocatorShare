using System.Web;
using System.Web.Optimization;

namespace AllocatorShare2
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new StyleBundle("~/bundles/css").Include(
                      "~/bower_components/react-treeview/react-treeview.css",
                        "~/Content/site.css"));
        }
    }
}
