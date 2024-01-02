using Project.MiningRenderingHost.Website.Models;
using Project.MiningRenderingHost.Website.Models.About_us;
using Project.MiningRenderingHost.Website.Models.Common;
using Project.MiningRenderingHost.Website.Models.ProjectDetails;
using Project.MiningRenderingHost.Website.Models.ProjectListing;
using Project.MiningRenderingHost.Website.Models.Sitemap;
using Sitecore.AspNet.RenderingEngine.Configuration;
using Sitecore.AspNet.RenderingEngine.Extensions;

namespace Project.MiningRenderingHost.Website.Extensions
{
    public static class RenderingEngineOptionsExtensions
    {
        public static RenderingEngineOptions AddPlatformComponent(this RenderingEngineOptions options)
        {
            options.AddModelBoundView<Header>("Header");
            options.AddModelBoundView<MainBannerCarousel>("MainBannerCarousel");
            options.AddModelBoundView<Banner>("Banner");
            options.AddModelBoundView<Breadcrumb>("Breadcrumb");
            options.AddModelBoundView<OurAccreditation>("OurAccreditation");
            options.AddModelBoundView<RequestAcall>("RequestAcall");
            options.AddModelBoundView<OurAccreditation>("OurAccreditation");
            options.AddModelBoundView<DiscoverMiningBrochureModel>("DiscoverMiningBrochure");
            options.AddModelBoundView<DiscoverMiningBrochureModel>("LeaderMessage");
            options.AddModelBoundView<DiscoverMiningBrochureModel>("WhyMTCS");
            options.AddModelBoundView<WhoWeAre>("WhoWeAre");
            options.AddModelBoundView<Services>("Services");
            options.AddModelBoundView<Partners>("Partners");
            options.AddModelBoundView<Projects>("Projects");
            options.AddModelBoundView<FAQ>("FAQ");
            options.AddModelBoundView<Footer>("Footer");
            options.AddModelBoundView<SubscribeUs>("SubscribeUs");
            options.AddModelBoundView<OurProjects>("OurProjects");
            options.AddModelBoundView<OtherProjects>("OtherProjects");
            options.AddModelBoundView<Breadcrumb>("Breadcrumb");
            options.AddModelBoundView<WhatWeStandFor>("WhatWeStandFor");
            options.AddModelBoundView<ProjectDetails>("ProjectDetails");
            options.AddModelBoundView<TermsandCondition>("TermsAndConditions");
            options.AddModelBoundView<PrivacyPolicy>("PrivacyPolicy");
            options.AddModelBoundView<SitemapModel>("Sitemap");
            return options;
        }
    }
}
