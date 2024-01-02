using Project.AAHL.Website.Models.Home;
using Project.AAHL.Website.Models.Our_Expertise;
using Project.AAHL.Website.Models.OurAirports;
using Project.AAHL.Website.Models.OurExpertise;
using Project.AAHL.Website.Models.OurStory;
using Sitecore.Mvc.Presentation;

namespace Project.AAHL.Website.Services.Common
{
    public interface IOurExpertise
    {
        RetailDevelopmentModel GetRetailDevelopmentModel(Rendering rendering);
        VideoBannerModel GetRetailVideoBannerModel(Rendering rendering);
    }
}
