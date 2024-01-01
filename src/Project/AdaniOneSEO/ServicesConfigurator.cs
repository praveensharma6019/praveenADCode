using Microsoft.Extensions.DependencyInjection;
using Project.AdaniOneSEO.Website.Services;
using Project.AdaniOneSEO.Website.Services.CityToCityPage;
using Project.AdaniOneSEO.Website.Services.FlightsToDestination;
using Project.AdaniOneSEO.Website.Services.FlightsToDestination.Filter_Options;
using Project.AdaniOneSEO.Website.Services.SitemapXml;
using Project.AdaniOneSEO.Website.Services.VideoGallery;
using Sitecore.DependencyInjection;

namespace Project.AdaniOneSEO.Website
{
    public class ServicesConfigurator : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IBannerServices, BannerServices>();
            serviceCollection.AddTransient<IDetailsServices, DetailsServices>();
            serviceCollection.AddTransient<IPopularFlights, PopularFlights>();
            serviceCollection.AddTransient<IFilterOptions, FilterOptions> ();
            serviceCollection.AddTransient<IPageMetaData, PageMetaData>();
            serviceCollection.AddTransient<IGetLowestPrice, GetLowestPrice>();
            serviceCollection.AddTransient<IVideoGalleryService, VideoGalleryService>();
            serviceCollection.AddTransient<ICityToCityPageService, CityToCityPageService>();
            serviceCollection.AddTransient<IBannerWidget, BannerWidget>();
            serviceCollection.AddTransient<ISitemapXmlService, SitemapXmlService>();
            serviceCollection.AddTransient<IAirportInformationService, AirportInformationService>();
            serviceCollection.AddTransient<ICityToCityPopularFlights, CityToCityPopularFlights>();
            serviceCollection.AddTransient<ICityToCityBannerWidgetService, CityToCityBannerWidgetService>();
        }
    }
}