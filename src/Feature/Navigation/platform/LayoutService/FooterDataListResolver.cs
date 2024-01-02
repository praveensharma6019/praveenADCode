using Adani.SuperApp.Airport.Feature.Navigation.Platform.Services;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Navigation.Platform.LayoutService
{
    public class FooterDataListResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
        private readonly IFooterDataList footerData;       
        public FooterDataListResolver(IFooterDataList footerData)
        {
            this.footerData = footerData;
        }
       
        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            //Get the datasource for the item 
            var datasource = RenderingContext.Current.Rendering.Item;
            // Null Check for datasource
          
                if (datasource == null)
                {
                    throw new NullReferenceException("FooterDataListResolver => Rendering Datasource is Empty");
                }
           
            return this.footerData.GetFooterData(datasource);
        }

    }


}
