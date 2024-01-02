﻿using Project.AAHL.Website.Services.AirportConcessions;
using Project.AAHL.Website.Services.Common;
using Project.AAHL.Website.Services.GeneralAviation;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Project.AAHL.Website.LayoutServiceContentResolvers.AirportConcessions
{
    public class BeyondAirportsResolver : RenderingContentsResolver
    {
        private readonly IAirportConcessionsServices rootResolver;

        public BeyondAirportsResolver(IAirportConcessionsServices airportConcessionsServices)
        {
            this.rootResolver = airportConcessionsServices;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return rootResolver.GetConcessions(rendering);
        }
    }
}