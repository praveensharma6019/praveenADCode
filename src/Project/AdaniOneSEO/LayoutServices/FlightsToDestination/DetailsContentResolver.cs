using Project.AdaniOneSEO.Website.Services.FlightsToDestination;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.AdaniOneSEO.Website.LayoutServices.FlightsToDestination
{
    public class DetailsContentResolver : RenderingContentsResolver
    {
        private readonly IDetailsServices RootResolver;

        public DetailsContentResolver(IDetailsServices Services)
        {
            this.RootResolver = Services;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return RootResolver.GetDetails(rendering);
        }
    }
}