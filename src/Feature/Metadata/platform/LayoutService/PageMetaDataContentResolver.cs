using Adani.SuperApp.Airport.Feature.MetaData.Platform.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;
using System;
namespace Adani.SuperApp.Airport.Feature.MetaData.Platform.LayoutService
{
    public class PageMetaDataContentResolver : RenderingContentsResolver
    {
        private readonly IPageMetaData _pageMetaData;
        public PageMetaDataContentResolver(IPageMetaData pageMetaData)
        {
            this._pageMetaData = pageMetaData;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            var datasource = RenderingContext.Current.ContextItem;
            if (datasource == null)
            {
                throw new NullReferenceException();
            }
            return this._pageMetaData.GetMetadata(datasource);

        }
    }
}