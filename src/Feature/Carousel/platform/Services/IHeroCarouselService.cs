using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Adani.SuperApp.Realty.Feature.Carousel.Platform.Models;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Adani.SuperApp.Realty.Feature.Carousel.Platform.Services
{
    public interface IHeroCarouselService
    {
        List<mediaCoverage> GetMediaCoverage(Rendering rendering);
        //HeroCarouselwidgets GetHeroCarouseldata(Rendering rendering);
        CarosuelList GetCarousuelList(Rendering rendering);
        List<Banner> GetBannerComponent(Rendering rendering);
        Banner GetCommunicationBanner(Rendering rendering);
        List<Banner> GetLocationBanner(Rendering rendering);
        List<AboutUsBanner> GetAboutUsBannerComponent(Rendering rendering);
        ResidentialProject GetResidentialBannerList(Rendering rendering);
        ClubLandingModel GetClubLanding(Rendering rendering);
        Shantigramhighlights GetShantigramhighlights(Rendering rendering);
        BrandIconList GetBrandIconList(Rendering rendering);
        List<Object> GetPropertyList(Rendering rendering, string location, string type, string status);
        List<Object> GetSeoPropertyList(Rendering rendering);
        List<Object> GetEnquiryFormProperty(Rendering rendering);
       // List<Object> GetPropertyList(Rendering rendering, string location);
        List<Object> GetTownshipPropertyList(Rendering rendering, string location);
        List<amenities> GetAmenities(Rendering rendering);
        OfficeData GetOfficeData(Rendering rendering);
        ProjectFoundStatus GetProjectLocation(Rendering rendering,string location);
        ContactUsPageData GetContactUsPageData(Rendering rendering);
    }
}
