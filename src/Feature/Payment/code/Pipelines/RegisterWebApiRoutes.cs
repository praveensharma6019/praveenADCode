﻿namespace Sitecore.Feature.Payment.Pipelines
{
    using System.Web.Mvc;
    using System.Web.Routing;
    using Sitecore.Pipelines;

    public class RegisterWebApiRoutes
  {
    public void Process(PipelineArgs args)
    {
      RouteTable.Routes.MapRoute("Feature.Payment.Api", "api/payment/{action}", new
                                                                                  {
                                                                                    controller = "Payment"
      });
    }
  }
}