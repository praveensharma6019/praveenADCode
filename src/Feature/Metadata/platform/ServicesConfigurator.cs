using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Adani.SuperApp.Airport.Feature.MetaData.Platform.Controllers;

namespace Adani.SuperApp.Airport.Feature.MetaData.Platform
{
    public class ServicesConfigurator : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            //serviceCollection.AddTransient<Services.IServiceList, Services.ServiceList>();
            serviceCollection.Replace(ServiceDescriptor.Scoped(typeof(HeadDataController),
                typeof(HeadDataController)));
            serviceCollection.AddTransient<Services.IPageMetaData, Services.PageMetaData>();

        }
    }
}