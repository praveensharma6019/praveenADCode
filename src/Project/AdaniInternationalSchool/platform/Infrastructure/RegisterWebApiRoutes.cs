using Sitecore.Pipelines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Project.AdaniInternationalSchool.Website.Infrastructure
{
    public class RegisterWebApiRoutes
    {
        public void Process(PipelineArgs args)
        {
            RouteTable.Routes.MapRoute("Project.AdaniInternationalSchool.Website.Infrastructure", "api/AISContactUs/{action}", new
            {
                controller = "AISContactUs"
            });
        }
    }
}