using Project.AdaniInternationalSchool.Website.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.AdaniInternationalSchool.Website.LayoutService
{
    
        public class OurApproachContentResolver : RenderingContentsResolver
        {
            private readonly IAdaniInternationalSchoolServices RootResolver; public OurApproachContentResolver(IAdaniInternationalSchoolServices ourApproachData)
            {
                this.RootResolver = ourApproachData;
            }
            public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
            {
                return RootResolver.GetOurApproach(rendering);
            }
        }
    
}
