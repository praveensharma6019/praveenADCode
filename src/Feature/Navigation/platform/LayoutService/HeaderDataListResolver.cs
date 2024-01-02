using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adani.SuperApp.Airport.Feature.Navigation.Platform.Services;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;

namespace Adani.SuperApp.Airport.Feature.Navigation.Platform.LayoutService
{
    public class HeaderDataListResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
        private readonly IHeaderDataList headerData;

        public HeaderDataListResolver(IHeaderDataList headerData)
        {
            this.headerData = headerData;
        }
       
        
        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            //Get the datasource for the item 
            var datasource = RenderingContext.Current.Rendering.Item;

            // Null Check for datasource
            if (datasource == null)
            {
                throw new NullReferenceException("HeaderDataListResolver => Rendering Datasource is Empty");
            }

            return this.headerData.GetHeaderData(datasource);

        }
    }
}