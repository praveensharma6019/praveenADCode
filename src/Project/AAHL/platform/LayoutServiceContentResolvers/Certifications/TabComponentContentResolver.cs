using Project.AAHL.Website.Services.Certifications;
using Project.AAHL.Website.Services.Common;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.AAHL.Website.LayoutServiceContentResolvers.Certifications
{
    public class TabComponentContentResolver : RenderingContentsResolver
    {
        private readonly ICertificationsService _rootResolver;

        public TabComponentContentResolver(ICertificationsService rootResolver)
        {
            _rootResolver = rootResolver;
        }   

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return _rootResolver.GetCertificate(rendering);
        }
    }
}