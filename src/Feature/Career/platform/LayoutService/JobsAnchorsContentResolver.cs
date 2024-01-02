using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using Adani.SuperApp.Realty.Feature.Career.Platform.Services;

namespace Adani.SuperApp.Realty.Feature.Career.Platform.LayoutService
{
    public class JobsAnchorsContentResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
        private readonly ICareerServices careerServices;

        public JobsAnchorsContentResolver(ICareerServices CareerServices)
        {
            this.careerServices = CareerServices;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
             return careerServices.GetJobsAnchorsList(rendering);
         
        }

    }
}