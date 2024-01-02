using Project.AAHL.Website.Models.AdvertisingSponsorships;
using Project.AAHL.Website.Models.Common;
using Project.AAHL.Website.Models.Home;
using Project.AAHL.Website.Models.OurStory;
using Sitecore.Mvc.Presentation;

namespace Project.AAHL.Website.Services.Common
{
    public interface IAdvertisingSponsorships
    {
        Advertisement GetAdvertisement(Rendering rendering);
        Partner GetPartner(Rendering rendering);
    }
}
