using Project.AAHL.Website.Services.OurBelief;
using Project.AAHL.Website.Services.OurStrategy;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.AAHL.Website.LayoutServiceContentResolvers.OurStrategy
{
    public class CardDetailWithImagesContentResolver : RenderingContentsResolver
    {
        private readonly IOurStrategy _rootResolver;

        public CardDetailWithImagesContentResolver(IOurStrategy rootResolver)
        {
            _rootResolver = rootResolver;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return _rootResolver.GetCardDetailWithImages(rendering);
        }
    }
}