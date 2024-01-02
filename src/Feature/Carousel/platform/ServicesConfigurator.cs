using Adani.SuperApp.Airport.Feature.Carousel.Platform.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Sitecore.DependencyInjection;
using Sitecore.Jobs.AsyncUI;

namespace Adani.SuperApp.Airport.Feature.Carousel.Platform
{
    public class ServicesConfigurator : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<Services.IHeroCarouselService, Services.HeroCarouselService>();
            serviceCollection.AddTransient<Services.IHeroSliderAppService, Services.HeroSliderAppService>();
            serviceCollection.AddTransient<Services.ISliderWithImageAndDetailService, Services.SliderWithImageAndDetailService>();
            serviceCollection.AddTransient<Services.ITitleDescImageDatasourceService, Services.TitleDescImageDatasourceService>();
            serviceCollection.AddTransient<Services.IBestSellerDutyFreeService, Services.BestSellerDutyFreeService>();
            serviceCollection.AddTransient<Services.IMoreServicesService, Services.MoreServicesService>();
            serviceCollection.AddTransient<Services.IOfferDiscount, Services.OfferDiscountService>();
            //Ticket NO  18854
            serviceCollection.AddTransient<Services.IRewardServices, Services.RewardServices>();
            serviceCollection.AddTransient<Services.IFAQService, Services.FAQService>();
            serviceCollection.AddTransient<Services.ITitleDescriptionWithLink, Services.TitleDescriptionWithLink>();
            serviceCollection.AddTransient<Services.IHeroSliderService, Services.HeroSliderService>();
            serviceCollection.AddTransient<Services.ILeftImageWithDetails, Services.LeftImageWithDetailsServices>();
            serviceCollection.AddTransient<Services.ITitleDescriptionWithLinkList, Services.TitleDescriptionWithLinkListService>();
            serviceCollection.AddTransient<Services.IMultiselectCarousel, Services.MultiselectCarousel>();
            // Ticket No 24723
            serviceCollection.AddTransient<Services.IRedeemOffer, Services.RedeemOfferServices>();
            serviceCollection.AddTransient<Services.IOffersAndDiscountsService, Services.OffersAndDiscountsService>();
            serviceCollection.AddTransient<Services.IThemeService, Services.ThemeService>();
            serviceCollection.AddTransient<Services.ITitleImageWithLink, Services.TitleImageWithLink>();
            serviceCollection.AddTransient<Services.ITitleImageDescriptionWithButton, Services.TitleImageDescriptionWithButton>();
            serviceCollection.AddScoped<Services.INotificationService, Services.NotificationService>();
            serviceCollection.AddScoped<Services.IPopularFlightRoutes, Services.PopularFlightRoutes>();
            serviceCollection.AddScoped<Services.IOurBusiness, Services.OurBusiness>();
            serviceCollection.AddScoped<Services.ITitleDescriptionWithDetailList, Services.TitleDescriptionWithDetailList>();
            serviceCollection.AddScoped<Services.IBaseCarousel, Services.BaseCarousel>();
            serviceCollection.AddScoped<Services.IServiceListService,Services.ServiceListService>();
            serviceCollection.AddScoped<Services.IFAQLandingPageService,Services.FAQLandingPageService>();
            serviceCollection.AddScoped<Services.IFAQSearchService, Services.FAQSearchService>();
        }
    }
}