﻿using Project.AdaniInternationalSchool.Website.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.AdaniInternationalSchool.Website.LayoutService
{
    public class InfrastructureCategoryContentResolver : RenderingContentsResolver
    {
        private readonly IAdaniInternationalSchoolServices RootResolver;

        public InfrastructureCategoryContentResolver(IAdaniInternationalSchoolServices InfrastructureData)
        {
            this.RootResolver = InfrastructureData;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return RootResolver.GetInfrastructureCategory(rendering);
        }
    }
}