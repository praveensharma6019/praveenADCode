using Microsoft.Extensions.DependencyInjection;
using Project.AAHL.Website.Services.AboutAHHL;
using Project.AAHL.Website.Services.AboutUs;
using Project.AAHL.Website.Services.AirportConcessions;
using Project.AAHL.Website.Services.Cargo;
using Project.AAHL.Website.Services.Certifications;
using Project.AAHL.Website.Services.Common;
using Project.AAHL.Website.Services.CompetetiveAdvantage;
using Project.AAHL.Website.Services.GeneralAviation;
using Project.AAHL.Website.Services.GroundTransportation;
using Project.AAHL.Website.Services.Investors;
using Project.AAHL.Website.Services.MediaCenter;
using Project.AAHL.Website.Services.OurBelief;
using Project.AAHL.Website.Services.OurLeadership;
using Project.AAHL.Website.Services.OurStrategy;
using Project.AAHL.Website.Services.SitemapXML;
using Sitecore.DependencyInjection;

namespace Project.AAHL.Website
{
    public class ServicesConfigurator : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IHomeService, HomeService>();
            serviceCollection.AddTransient<IHeaderService, HeaderService>();
            serviceCollection.AddTransient<IAwardsService, AwardsService>();
            serviceCollection.AddTransient<IPartnership, Partnerships>();
            serviceCollection.AddTransient<IFooter, Footer>();
            serviceCollection.AddTransient<IAboutUsServices, AboutUsServices>();
            serviceCollection.AddTransient<IAboutAHHLServices, AboutAHHLServices>();
            serviceCollection.AddTransient<IOurLeadersServices, OurLeadersServices>();
            serviceCollection.AddTransient<IFooterNew, FooterNew>();
            serviceCollection.AddTransient<IOurBeliefService, OurBeliefService>();
            serviceCollection.AddTransient<IOurStrategy, OurStrategy>();
            serviceCollection.AddTransient<IOurAirports, OurAirports>();
            serviceCollection.AddTransient<ICompetetiveAdvantageService, CompetetiveAdvantageService>();
            serviceCollection.AddTransient<ICertificationsService, CertificationsService>();
            serviceCollection.AddTransient<IGeneralAviationServices, GeneralAviationServices>();
            serviceCollection.AddTransient<IGroundTransportationServices, GroundTransportationServices>();
            serviceCollection.AddTransient<ISustainability, Sustainability>();
            serviceCollection.AddTransient<IOurExpertise, OurExpertise>();
            serviceCollection.AddTransient<IAdvertisingSponsorships, AdvertisingSponsorships>();
            serviceCollection.AddTransient<IContactUsServices, ContactUsServices>();
            serviceCollection.AddTransient<ICargoServices, CargoServices>();
            serviceCollection.AddTransient<IAirportConcessionsServices, AirportConcessionsServices>();
            serviceCollection.AddTransient<IContactUsServices, ContactUsServices>();
            serviceCollection.AddTransient<IOurStory, OurStory>();
            serviceCollection.AddTransient<IMediaCenterServices, MediaCenterServices>();
            serviceCollection.AddTransient<ICookiesService, CookiesService>();
            serviceCollection.AddTransient<IInvestorsServices, InvestorsServices>();
            serviceCollection.AddTransient<ISitemapXMLService, SitemapXMLService>();
        }
    }
}