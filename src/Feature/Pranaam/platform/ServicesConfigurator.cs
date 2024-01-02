using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;
using Adani.SuperApp.Airport.Feature.Pranaam.Models;

namespace Adani.SuperApp.Airport.Feature.Pranaam
{
    public class ServicesConfigurator: IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<Services.IAddOnServiceContent, Services.AddOnServiceContent>();
            serviceCollection.AddTransient<Services.IBookingSteps, Services.BookingStepsRepository>();
            serviceCollection.AddTransient<Services.ICustomerFeedback, Services.CustomerFeedbackRepository>();
            serviceCollection.AddTransient<Services.IPorterService, Services.PorterRepository>();
            serviceCollection.AddTransient<Services.IServiceDemo, Services.ServiceDemoRepository>();
            serviceCollection.AddTransient<Services.IDepartureLandingContent, Services.DepartureLandingContent>();
            serviceCollection.AddTransient<Services.IDepartureCarouselContent, Services.DepartureCarouselContent>();
            serviceCollection.AddTransient<Services.IAddOnServiceTab, Services.AddOnServiceTabRepository>();
            serviceCollection.AddTransient<Services.IPackages, Services.PackagesRepository>();
            serviceCollection.AddTransient<Services.IPackageServices, Services.PackageServicesRepository>();
            serviceCollection.AddTransient<Services.IServiceCarousal, Services.ServiceCarousalRepository>();
            serviceCollection.AddTransient<Services.IAirportPackagesContent, Services.AirportPackagesContent>();
            serviceCollection.AddTransient<Services.IPackageIDRepository, Services.PackageIDRepository>();
            serviceCollection.AddTransient<Services.IServiceTypePackages, Services.ServiceTypePackagesRepository>();
            serviceCollection.AddTransient<Services.IServiceBookingForm, Services.ServiceBookingFormRepository>();
            serviceCollection.AddTransient<Services.IHowItWorkSteps, Services.HowItWorkSteps>();
            serviceCollection.AddTransient<Services.ICancellation, Services.Cancellation>();
            serviceCollection.AddTransient<Services.IFooterIllustration, Services.FooterIllustrationContent>();
            serviceCollection.AddTransient<Services.IStandaloneProducts, Services.StandaloneProducts>();
        }
    }
}