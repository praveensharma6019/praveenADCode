﻿using Adani.SuperApp.Realty.Feature.Widget.Platform.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Realty.Feature.Widget.Platform.LayoutService
{
    public class ProjectActionResolver : RenderingContentsResolver
    {
        private readonly IJobDetailService jobdetailService;
        public ProjectActionResolver(IJobDetailService jobdetailService)
        {
            this.jobdetailService = jobdetailService;
        }
        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return jobdetailService.GetProjectAction(rendering.Item);

        }
    }
}