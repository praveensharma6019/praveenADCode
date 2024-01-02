using Adani.SuperApp.Realty.Feature.Career.Platform.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Realty.Feature.Career.Platform.LayoutService
{
    public class AboutCareerContentResolver : RenderingContentsResolver
    {
        private readonly IAboutCareerServices RootResolver;

        public AboutCareerContentResolver(IAboutCareerServices aboutCareerServices)
        {
            this.RootResolver = aboutCareerServices;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return RootResolver.GetAboutCareer(rendering);

        }
    }
}