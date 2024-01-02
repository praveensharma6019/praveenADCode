using Adani.SuperApp.Realty.Feature.Configuration.Platform.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Realty.Feature.Configuration.Platform.LayoutService
{
    public class PolicyContentResolver: RenderingContentsResolver
    {
       
            private readonly IPolicyService policyService;
            public PolicyContentResolver(IPolicyService policyService)
            {
                this.policyService = policyService;
            }
            public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
            {
                return policyService.GetPolicyData(rendering);

            }
        }
}