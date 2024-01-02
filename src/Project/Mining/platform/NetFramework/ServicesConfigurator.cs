using Microsoft.Extensions.DependencyInjection;
using Project.Mining.Website.Banner;
using Project.Mining.Website.Home;
using Project.Mining.Website.Services;
using Project.Mining.Website.Services.Banner;
using Project.Mining.Website.Services.Common;
using Project.Mining.Website.Services.ContactUsPage;
using Project.Mining.Website.Services.Forms;
using Project.Mining.Website.Services.Header;
using Project.Mining.Website.Services.Home;
using Project.Mining.Website.Services.OurAccreditation;
using Project.Mining.Website.Services.OurProjects;
using Project.Mining.Website.Services.OurServices;
using Project.Mining.Website.Services.ProjectListing;
using Project.Mining.Website.Services.Sitemap;
using Sitecore.DependencyInjection;

namespace Project.Mining.Website
{
    public class ServicesConfigurator : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IHeaderService, HeaderService>();
            serviceCollection.AddTransient<IBannerService, BannerService>();
            serviceCollection.AddTransient<IOurAccreditationService, OurAccreditationService>();
            serviceCollection.AddTransient<IWhoWeAreService, WhoWeAreService>();
            serviceCollection.AddTransient<IOurServices, OurServices>();
            serviceCollection.AddTransient<IDiscoverMiningBrochureServices, DiscoverMiningBrochureServices>();
            serviceCollection.AddTransient<IWhatWeStandForService, WhatWeStandForService>();
            serviceCollection.AddTransient<IWhyMtcsService, WhyMtcsService>();
            serviceCollection.AddTransient<IBreadcrumbService, BreadcrumbService>();
            serviceCollection.AddTransient<IProjectDetailsService, ProjectDetailsService>();
            serviceCollection.AddTransient<IOtherProjectsService, OtherProjectsService>();
            serviceCollection.AddTransient<ISubscribeUsService, SubscribeUsService>();
            serviceCollection.AddTransient<IProjectListingService, ProjectListingService>();
            serviceCollection.AddTransient<IFooterService, FooterService>();
            serviceCollection.AddTransient<IMiningFormsService, MiningFormsService>();
            serviceCollection.AddTransient<IPrivacyPolicyService, PrivacyPolicyService>();
            serviceCollection.AddTransient<ISitemapService, SitemapService>();
            serviceCollection.AddTransient<IContactUsPageService, ContactUsPageService>();
        }
    }
}