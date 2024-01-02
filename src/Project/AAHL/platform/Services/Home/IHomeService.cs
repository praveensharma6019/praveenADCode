using Project.AAHL.Website.Models.Home;
using Sitecore.Mvc.Presentation;

namespace Project.AAHL.Website.Services.Common
{
    public interface IHomeService
    {
        PageMetaData GetPageMetaData(Rendering rendering);
        Banner GetBanner(Rendering rendering);
        AirportStats GetAirportStats(Rendering rendering);
        BannerAdsModel GetBannerAdsModel(Rendering rendering);
        AirportNews GetAirportNews(Rendering rendering);
        AboutAirport GetAboutAirport(Rendering rendering);
        AirportBusiness GetAirportBusiness(Rendering rendering);
      
    }
}
