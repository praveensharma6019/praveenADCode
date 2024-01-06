using Sitecore.Pipelines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Sitecore.Agrilogistics.Website.Infrastructure
{
    public class RegisterWebApiRoutes
    {
        public void Process(PipelineArgs args)
        {
            RouteTable.Routes.MapRoute("Sitecore.Agrilogistics.Website.Infrastructure", "api/Agrilogistics/{action}", new
            {
                controller = "Agrilogistics"
            });
        }
    }
}