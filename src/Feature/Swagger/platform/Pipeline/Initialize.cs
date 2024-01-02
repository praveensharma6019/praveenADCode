using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Pipelines;

namespace Adani.SuperApp.Airport.Feature.Swagger.Platform.Pipeline
{
    public class InitializeSwagger
    {
        public void Process(PipelineArgs args)
        {
            SwaggerConfig.Register();
        }
    }
}