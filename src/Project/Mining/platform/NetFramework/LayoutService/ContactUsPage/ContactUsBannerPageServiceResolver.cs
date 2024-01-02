using Project.Mining.Website.Services.Banner;
using Project.Mining.Website.Services.ContactUsPage;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Mining.Website.LayoutService.ContactUsPage
{
    public class ContactUsBannerPageServiceResolver : RenderingContentsResolver
    {
        private readonly IContactUsPageService _rootResolver;

        public ContactUsBannerPageServiceResolver(IContactUsPageService rootResolver)
        {
            _rootResolver = rootResolver;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return _rootResolver.GetContactUsBanner(rendering);

        }
    }
}