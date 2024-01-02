using Project.AAHL.Website.Services.OurBelief;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.AAHL.Website.LayoutServiceContentResolvers.OurBelief
{
    public class OurPurposeContentResolver : RenderingContentsResolver
    {
        private readonly IOurBeliefService _rootResolver;

        public OurPurposeContentResolver(IOurBeliefService rootResolver)
        {
            _rootResolver = rootResolver;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return _rootResolver.GetOurPurpose(rendering);
        }
    }
}