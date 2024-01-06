using Sitecore.Pipelines;
using System.Web.Mvc;
using System.Web.Routing;

namespace Sitecore.Feature.BlobStorage.Pipelines
{
    public class RegisterWebApiRoutes
    {
        public void Process(PipelineArgs args)
        {

            //RouteTable.Routes.MapRoute("BlobStorageFileUploadAPI",
            //  "api/BlobStorage/UploadFileToBlobStorage",
            //  new
            //  {
            //      controller = "BlobStorage",
            //      action = "UploadFileToBlobStorage"
            //  });
            RouteTable.Routes.MapRoute("Sitecore.Feature.BlobStorage.Pipelines", "api/BlobStorage/{action}", new
            {
                controller = "BlobStorage"
            });

        }
    }
}