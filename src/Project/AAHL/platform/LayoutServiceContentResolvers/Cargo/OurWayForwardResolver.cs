﻿using Project.AAHL.Website.Services.Cargo;
using Project.AAHL.Website.Services.Common;
using Project.AAHL.Website.Services.GeneralAviation;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Project.AAHL.Website.LayoutServiceContentResolvers.Cargo
{
    public class OurWayForwardResolver : RenderingContentsResolver
    {
        private readonly ICargoServices rootResolver;

        public OurWayForwardResolver(ICargoServices cargoServices)
        {
            this.rootResolver = cargoServices;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return rootResolver.GetOurWayForward(rendering);
        }
    }
}