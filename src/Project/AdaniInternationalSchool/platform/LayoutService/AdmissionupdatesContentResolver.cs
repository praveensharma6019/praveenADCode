using Project.AdaniInternationalSchool.Website.AdmissionPage;
using Project.AdaniInternationalSchool.Website.LearningPage;
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
    public class AdmissionupdatesContentResolver : RenderingContentsResolver
    {
        private readonly IAISAdmissionPageServices RootResolver;

        public AdmissionupdatesContentResolver(IAISAdmissionPageServices aboutCareerServices)
        {
            this.RootResolver = aboutCareerServices;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return RootResolver.GetAdmissionupdates(rendering);
        }
    }
}