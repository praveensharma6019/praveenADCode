using Adani.SuperApp.Airport.Feature.Forex.Platform.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Forex.Platform.LayoutServices
{
    public class ImportantInformationResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    
    {
        private readonly IImportantInformation importantInformationJSON;

        public ImportantInformationResolver(IImportantInformation _importantInformationJSON)
        {
            this.importantInformationJSON = _importantInformationJSON;
        }

        public override object ResolveContents(Sitecore.Mvc.Presentation.Rendering rendering , IRenderingConfiguration renderingConfig)
        {
            return this.importantInformationJSON.GetForexImportantInfo(rendering);
        }

    }
}