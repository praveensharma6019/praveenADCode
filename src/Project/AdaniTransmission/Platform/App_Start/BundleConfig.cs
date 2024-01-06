
using System;
using System.Collections.Generic;
using Sitecore;
using Sitecore.Pipelines;
using System.Web.Optimization;

namespace Adani.BAU.Transmission.Project.Platform.Pipelines
{
    public class BundleConfig   
    {
        

        public virtual void Process(PipelineArgs args)
        {
            RegisterBundles(BundleTable.Bundles);
        }

        public  void RegisterBundles(BundleCollection bundles)
        {            

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/assets/airportservices/statics/js/jquery.validate.js",
                        "~/assets/airportservices/statics/js/jquery.validate.min.js"));

            BundleTable.EnableOptimizations = true;
        }
    }
}