using Adani.EV.Project.Models;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.EV.Project.Services
{
    public interface IHomeService
    {
     
        HeroCarouselModel GetHeroCarousel(Rendering rendering);
        QuickInfoModel GetQuickInfoModel(Rendering rendering);
        QuickLinkModel GetQuickLinkModel(Rendering rendering);
        WhySearchWithUsModel GetWhySearchWithUsModel(Rendering rendering);
        LatestEVNewsModel GetLatestEVNewsModel(Rendering rendering);
        EVNearBannerModel GetEVNearBannerModelModel(Rendering rendering);
        ChargingStationBannerModel GetChargingStationBannerModel(Rendering rendering);

        FaqModel GetFaqModel(Rendering rendering);
        ContactusInfoModel GetContactusInfoModel(Rendering rendering);

        
    }
}
