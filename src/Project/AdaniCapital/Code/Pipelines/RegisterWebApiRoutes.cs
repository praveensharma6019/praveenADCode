using Sitecore.Pipelines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Sitecore.AdaniCapital.Website.Pipelines
{
    public class RegisterWebApiRoutes
    {
        public void Process(PipelineArgs args)
        {
            RouteTable.Routes.MapRoute("Sitecore.AdaniCapital.Website.Pipelines", "api/AdaniCapital/{action}", new
            {
                controller = "AdaniCapital"
            });


        }
    }
}