// Decompiled with JetBrains decompiler
// Type: Sitecore.AdaniGreenTalks.Website.Infrastructure.RegisterWebApiRoutes
// Assembly: Sitecore.AdaniGreenTalks.Website, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E01E7F81-DBA4-49B5-BF38-8DFD57A2798A
// Assembly location: D:\Deployments\Stage\Sitecore.AdaniGreenTalks.Website.dll

using Sitecore.Pipelines;
using System.Web.Mvc;
using System.Web.Routing;

namespace Sitecore.AdaniGreenTalks.Website.Infrastructure
{
  public class RegisterWebApiRoutes
  {
    public void Process(PipelineArgs args) => RouteTable.Routes.MapRoute("Sitecore.AdaniGreenTalks.Website.Infrastructure", "api/AdaniGreenTalks/{action}", (object) new
    {
      controller = "AdaniGreenTalks"
    });
  }
}
