﻿using Project.AdaniInternationalSchool.Website.Services.ContentResolver;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Project.AdaniInternationalSchool.Website.LayoutService
{
    public class VisitOurSchoolContentResolver : RenderingContentsResolver
    {
        private readonly IContentResolverService _contentResolverService;
        public VisitOurSchoolContentResolver(IContentResolverService contentResolverService)
        {
            _contentResolverService = contentResolverService;
        }
        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return _contentResolverService.GetVisitOurSchoolCardModel(rendering);
        }
    }
}