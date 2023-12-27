using Sitecore.Mvc.Pipelines.Loader;
using Sitecore.Pipelines;
using System.Web.Optimization;


namespace Adani.SuperApp.Airport.Project.Airport.Platform.Pipelines
{
    public class BundleConfig : InitializeRoutes
    {

        public override void Process(PipelineArgs args)
        {
            RegisterBundles(BundleTable.Bundles);
        }

        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new Bundle("~/assets/css/about").Include(
            "~/assets/css/about.min.css"));

            bundles.Add(new Bundle("~/assets/css/Landing").Include(
            "~/assets/css/Landing.min.css"));

            bundles.Add(new Bundle("~/assets/js/landing").Include(
               "~/assets/js/landing.min.js"));

            bundles.Add(new Bundle("~/assets/css/Offers").Include(
            "~/assets/css/Offers.min.css"));

            bundles.Add(new Bundle("~/assets/js/offers").Include(
               "~/assets/js/offers.min.js"));

            bundles.Add(new Bundle("~/assets/css/static").Include(
              "~/assets/css/static.min.css"));

            bundles.Add(new Bundle("~/assets/css/AirportMain").Include(
             "~/assets/css/AirportMain.min.css"));

            bundles.Add(new Bundle("~/assets/js/AirportMain").Include(
               "~/assets/js/AirportMain.min.js"));

            bundles.Add(new Bundle("~/assets/js/AirportOther").Include(
               "~/assets/js/AirportOther.min.js"));

            bundles.Add(new Bundle("~/assets/css/AirportHome").Include(
            "~/assets/css/AirportHome.min.css"));

            bundles.Add(new Bundle("~/assets/js/AirportHome").Include(
               "~/assets/js/AirportHome.min.js"));

            bundles.Add(new Bundle("~/assets/css/staticservices").Include(
            "~/assets/css/staticservices.min.css"));

            bundles.Add(new Bundle("~/assets/css/citytocity").Include(
           "~/assets/css/citytocity.min.css"));

            bundles.Add(new Bundle("~/assets/css/citytocitylanding").Include(
           "~/assets/css/citytocitylanding.min.css"));            

            BundleTable.EnableOptimizations = true;

        }
    }
}