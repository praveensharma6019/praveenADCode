﻿using Adani.SuperApp.Realty.Feature.JSSComponents.Platform.Services.Common;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;


namespace Adani.SuperApp.Realty.Feature.JSSComponents.Platform.LayoutService
{
    public class HomeCurriculumContentResolver : RenderingContentsResolver
    {
        private readonly ICommonComponents RootResolver;

        public HomeCurriculumContentResolver(ICommonComponents homeCurriculumData)
        {
            this.RootResolver = homeCurriculumData;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return RootResolver.GetCurriculum(rendering);
        }
    }
}