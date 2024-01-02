using Adani.SuperApp.Realty.Feature.ContentSnippets.Platform.Services;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;

namespace Adani.SuperApp.Realty.Feature.ContentSnippets.Platform
{
    public class ServicesConfigurator : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IAboutAdaniRealtyRootResolverService, AboutAdaniRealtyRootResolverService>();
            serviceCollection.AddTransient<IAboutUsRootResolverService, AboutUsRootResolverService>();
            serviceCollection.AddTransient<ILocationRootResolverService, LocationRootResolverService>();
            serviceCollection.AddTransient<IOurLocationRootResolverService, OurLocationRootResolverService>();
            serviceCollection.AddTransient<IGetInTouchRootResolverService, GetInTouchRootResolverService>();
            serviceCollection.AddTransient<IGoodLifeRootResolverService, GoodLifeRootResolverService>();
            serviceCollection.AddTransient<IAboutGoodLifeRootResolverService, AboutGoodLifeRootResolverService>();
            serviceCollection.AddTransient<IFeaturedBlogRootResolverService, FeaturedBlogRootResolverService>();
            serviceCollection.AddTransient<IAboutCityRootResolverService,   AboutCityRootResolverService>();
            serviceCollection.AddTransient<IStaticTextRootResolverService, StaticTextRootResolverService>();
            serviceCollection.AddTransient<IRoomDetailsRootResolverService, RoomDetailsRootResolverService>();
            serviceCollection.AddTransient<IClubDetailsRootResolverService, ClubDetailsRootResolverService>();
            serviceCollection.AddTransient<IOrderConfirmationRootResolverService, OrderConfirmationRootResolverService>();
            
        }
    }
}