using Project.AdaniInternationalSchool.Website.Services.AboutUs;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.AdaniInternationalSchool.Website.LayoutService
{
    public class AboutUsServiceResolver : RenderingContentsResolver
    {
        private readonly IAboutUsService rootResolver;

        public AboutUsServiceResolver(IAboutUsService aboutusResolver)
        {
            this.rootResolver = aboutusResolver;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
          return rootResolver.GetAboutUsModel(rendering);

            //return "test";

        }
    }
}