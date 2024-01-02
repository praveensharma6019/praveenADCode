using Project.AmbujaCement.Website.Models.AboutUsPage;
using Project.AmbujaCement.Website.Models.DealerLocatorPage;
using Project.AmbujaCement.Website.Models.ProductList;
using Sitecore.Mvc.Presentation;

namespace Project.AmbujaCement.Website.Services.AboutUs
{
    public interface IDealerLocatorListService
    {
        DealerLocatorDataModel GetDealerLocatorData(Rendering rendering);
        DealerLocatorDetailsModel GetDealerDetailResponse(Rendering rendering);
        //DealerLocatorDataModel GetDealerResponse(Rendering rendering, string stateId="", string areaId = "", string districtId = "");
    }
}
