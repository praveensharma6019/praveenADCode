using Microsoft.Extensions.DependencyInjection;
using Project.AmbujaCement.Website.Services;
using Project.AmbujaCement.Website.Services.AboutUs;
using Project.AmbujaCement.Website.Services.Common;
using Project.AmbujaCement.Website.Services.CostCalculator;
using Project.AmbujaCement.Website.Services.CostCalculatorClientAPI;

using Project.AmbujaCement.Website.Services.DealerResult;
using Project.AmbujaCement.Website.Services.FAQ;

using Project.AmbujaCement.Website.Services.Forms;
using Project.AmbujaCement.Website.Services.GetInTouch;
using Project.AmbujaCement.Website.Services.Header;
using Project.AmbujaCement.Website.Services.Home;
using Project.AmbujaCement.Website.Services.HomeBuilder;
using Project.AmbujaCement.Website.Services.Sitemap;
using Project.AmbujaCement.Website.Services.SitemapXML;
using Sitecore.DependencyInjection;

namespace Project.AmbujaCement.Website
{
    public class ServicesConfigurator : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IHomeServices, HomeServices>();
            serviceCollection.AddTransient<IFooterServices, FooterServices>();
            serviceCollection.AddTransient<IHeaderServices, HeaderServices>();
            serviceCollection.AddTransient<IBannerService, BannerService>();
            serviceCollection.AddTransient<IBreadCrumbService, BreadCrumbService>();
            serviceCollection.AddTransient<IPageDetailsService, PageDetailsService>();
            serviceCollection.AddTransient<IGetInTouchService, GetInTouchService>();
            serviceCollection.AddTransient<ISitemapService, SitemapService>();
            serviceCollection.AddTransient<IAmbujaFormsService, AmbujaFormsService>();
            serviceCollection.AddTransient<IAboutUsService, AboutUsService>();


            serviceCollection.AddTransient<ICostCalculatorServices, CostCalculatorServices>();
            serviceCollection.AddTransient<ICostCalculatorClientAPIServices, CostCalculatorClientAPIServices>();


            serviceCollection.AddTransient<IDealerLocatorListService, DealerLocatorService>();
            serviceCollection.AddTransient<IProjectDetailsService, ProjectDetailsService>();

            serviceCollection.AddTransient<IProductListService, ProductListService>();


            serviceCollection.AddTransient<IDealerResultService, DealerResultService>();
            serviceCollection.AddTransient<IFAQService, FAQService>();


            serviceCollection.AddTransient<ISitemapXMLService, SitemapXMLService>();
            serviceCollection.AddTransient<ISubNavService, SubNavService>();
            serviceCollection.AddTransient<IChecklistService, ChecklistService>();
        }
    }
}