using Sitecore.LayoutService.Configuration;
using System;
using Sitecore.Mvc.Presentation;
using Adani.SuperApp.Airport.Feature.Master.Platform.Services;

namespace Adani.SuperApp.Airport.Feature.Master.Platform.LayoutService
{
    public class StateMasterResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
        private readonly IStateMasterService stateMasterService;

        public StateMasterResolver(IStateMasterService stateMasterService)
        {
            this.stateMasterService = stateMasterService;
        }
        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            var datasource = RenderingContext.Current.Rendering.Item;
            // Null Check for datasource
            if (datasource == null)
            {
                throw new NullReferenceException();
            }
            string CountryId = Sitecore.Context.Request.QueryString["countryid"];
            return this.stateMasterService.GetStateMasterData(datasource, CountryId);
        }
    }
}