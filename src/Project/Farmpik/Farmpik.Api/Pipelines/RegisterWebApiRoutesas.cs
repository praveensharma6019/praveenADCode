using Sitecore.Pipelines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Sitecore.Farmpik.Api.Website.Pipelines
{
    public class RegisterWebApiRoutesas
    {
        public void Process(PipelineArgs args)
        {
            RouteTable.Routes.MapRoute("Sitecore.Farmpik.Api.Website.Pipelines", "api/Product/{action}",
                new
                {
                    controller = "Product"
                });

        }



    }
}