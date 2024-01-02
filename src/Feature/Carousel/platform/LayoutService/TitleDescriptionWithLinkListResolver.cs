using Adani.SuperApp.Airport.Feature.Carousel.Platform.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Carousel.Platform.LayoutService
{
    public class TitleDescriptionWithLinkListResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
        private readonly ITitleDescriptionWithLinkList titleDescriptionWithLinkLists;
        public TitleDescriptionWithLinkListResolver(ITitleDescriptionWithLinkList titleDescriptionWithLinkList)
        {
            this.titleDescriptionWithLinkLists = titleDescriptionWithLinkList;
        }
        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            //Get the datasource for the item
            var datasource = RenderingContext.Current.Rendering.Item;
            // Null Check for datasource
            if (datasource == null)
            {
                throw new NullReferenceException();
            }

            return this.titleDescriptionWithLinkLists.TitleDescriptionWithLinkListData(rendering, datasource);

        }
    }
}