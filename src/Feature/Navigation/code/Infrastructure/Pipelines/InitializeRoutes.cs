using Sitecore.Pipelines;
using System.Web.Routing;

namespace Sitecore.Feature.Navigation.Infrastructure.Pipelines
{
    public class InitializeRoutes
    {
        public void Process(PipelineArgs args)
        {
            if (!Context.IsUnitTesting)
            {
                RouteConfig.RegisterRoutes(RouteTable.Routes);
            }
        }
    }
}
