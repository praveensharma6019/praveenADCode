using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;


namespace Adani.SuperApp.Airport.Feature.Retail.Platform
{
    public class ServicesConfigurator : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<Services.IRetailBrandsDetails, Services.RetailBrandsDetails>();
            serviceCollection.AddScoped<Services.IRetailOutlet, Services.RetailOutlet>();
            serviceCollection.AddScoped<Services.IPopularCategory, Services.PopularCategory>();
            serviceCollection.AddScoped<Services.IBestOffers, Services.BestOffers>();
            serviceCollection.AddScoped<Services.IRefundPolicy, Services.RefundPolicy>();
            serviceCollection.AddScoped<Services.ISizeSelect, Services.SizeSelect>();
        }
    }
}