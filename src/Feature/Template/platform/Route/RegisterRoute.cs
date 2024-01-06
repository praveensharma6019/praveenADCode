using Sitecore.Pipelines;
using System.Web.Mvc;
using System.Web.Routing;

namespace Sitecore.Feature.Template.Route
{
    public class RegisterRoute
    {
        public void Process(PipelineArgs args)
        {
            //CreateItem Route
            RouteTable.Routes.MapRoute("TemplateServiceApi",
               "abcd/TemplateItem/CreateItem",
               new
               {
                   controller = "TemplateItem",
                   action = "CreateItem"
               });

            //UpdateItem Route
            RouteTable.Routes.MapRoute("UpdateItemServiceApi",
               "abcd/TemplateItem/UpdateItem",
               new
               {
                   controller = "TemplateItem",
                   action = "UpdateItem"
               });
        }
    }
}