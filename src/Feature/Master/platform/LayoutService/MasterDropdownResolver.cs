using Adani.SuperApp.Airport.Feature.Master.Platform.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;
using System;

namespace Adani.SuperApp.Airport.Feature.Master.Platform.LayoutService
{
    public class MasterDropdownResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
        private readonly IMasterDropdownService masterDropdownService;

        public MasterDropdownResolver(IMasterDropdownService masterDropdownService)
        {
            this.masterDropdownService = masterDropdownService;
        }
        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            var datasource = RenderingContext.Current.Rendering.Item;
            // Null Check for datasource
            if (datasource == null)
            {
                throw new NullReferenceException();
            }
            return this.masterDropdownService.GetMasterDropdownData(datasource);
        }
    }
}