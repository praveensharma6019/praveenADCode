using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;


namespace Adani.SuperApp.Airport.Feature.FNB.Platform
{
    public class ServicesConfigurator : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
           
            serviceCollection.AddScoped<Services.IFnBExclusiveOutlet, Services.FnBExclusiveOutlet>();
            serviceCollection.AddScoped<Services.ISapAPI, Services.SapAPI>();
            serviceCollection.AddScoped<Services.IOutletStatus, Services.OutletStatus>();
            serviceCollection.AddScoped<Services.ITitleDescriptionAPI, Services.TitleDescriptionAPI>();
            serviceCollection.AddScoped<Services.IFnBbankoffers, Services.FnBbankoffers>();
            serviceCollection.AddScoped<Services.ITermsAndConditionJSON, Services.TermsAndConditionJSON>();
            serviceCollection.AddScoped<Services.ICarouselOutlet, Services.CarouselOutlet>();
            serviceCollection.AddScoped<Services.IOffersOutlet, Services.OffersOutlet>();
            serviceCollection.AddScoped<Services.IImageMobileImageInterface, Services.ImgMobileImageService>();
            serviceCollection.AddScoped<Services.ICartFNBservice, Services.CartFNBservice>();
            serviceCollection.AddScoped<Services.ICancellationReasons, Services.CancellationReasons>();
            serviceCollection.AddScoped<Services.IBannerDetails, Services.BannerDetails>();
            serviceCollection.AddScoped<Services.IFAQ, Services.FAQ>();


        }
    }
}