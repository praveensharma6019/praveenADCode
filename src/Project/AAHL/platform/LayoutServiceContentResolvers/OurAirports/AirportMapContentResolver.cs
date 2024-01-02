﻿using Project.AAHL.Website.Services.Common;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Project.AAHL.Website.LayoutServiceContentResolvers
{
    public class AirportMapContentResolver : RenderingContentsResolver
    {
        private readonly IOurAirports rootResolver;

        public AirportMapContentResolver(IOurAirports dataResolver)
        {
            this.rootResolver = dataResolver;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return rootResolver.GetAirportMap(rendering);
        }
    }
}