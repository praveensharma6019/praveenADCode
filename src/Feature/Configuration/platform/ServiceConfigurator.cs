using Adani.SuperApp.Realty.Feature.Configuration.Platform.Services;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;

namespace Adani.SuperApp.Realty.Feature.Configuration.Platform
{
    public class ServiceConfigurator : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<Services.ICommonDataService, Services.CommonDataService>();
            serviceCollection.AddTransient<Services.ICommunicationCornerService, Services.CommunicationCornerService>();
            serviceCollection.AddTransient<Services.IBlogContentService, Services.BlogContentService>();
            serviceCollection.AddTransient<Services.IQuickLinksService, Services.QuickLinksService>();
            serviceCollection.AddTransient<Services.IWhyAdaniService, Services.WhyAdaniService>();
            serviceCollection.AddTransient<Services.IPolicyService, Services.PolicyService>();
            serviceCollection.AddScoped<IMasterListService, MasterListService>();
        }
    }
}