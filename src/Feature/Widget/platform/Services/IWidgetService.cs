using Adani.SuperApp.Realty.Feature.Widget.Platform.Models;
using Sitecore.Mvc.Presentation;
using System.Collections.Generic;

namespace Adani.SuperApp.Realty.Feature.Widget.Platform.Services
{
    public interface IWidgetService
    {
        FeedsdataList GetFeedsList(Sitecore.Mvc.Presentation.Rendering rendering);
        AllaccoladesList GetAllaccoladesList(Sitecore.Mvc.Presentation.Rendering rendering);
        TownshipList GetTownshipItem(Sitecore.Mvc.Presentation.Rendering rendering);
        TimelineList GetTimelineList(Sitecore.Mvc.Presentation.Rendering rendering);
        OurBrand GetOurBrandsList(Rendering rendering);
        Location GetLocationDataList(Rendering rendering);
        EmployeeCard GetEmployeeCardDataList(Rendering rendering);
        PageContent GetPageContentData(Rendering rendering);
        CategoryLifestyleList GetCatagoryLIfestylItem(Rendering rendering);
        CommunicationItems GetCommunicationCornerItem(Rendering rendering);
        RestaurantInformation GetRestaurantInformation(Rendering rendering);
        List<ReasaurantCard> GetReasaurantCard(Rendering rendering);
        List<ReasaurantMenu> GetReasaurantMenu(Rendering rendering);

        RestaurantTabData GetRestaurantTabData(Rendering rendering);

        ReasaurantContent GetRestaurantContentData(Rendering rendering);

        TopBarModel GetTopBarData(Rendering rendering);

        RoomTitleModel GetRoomTitleData(Rendering rendering);

        RoomInfoTabInfos GetRoomInfoTabData(Rendering rendering);

        RoomFacilities GetFacilitiesData(Rendering rendering);

        OtherRoomsModel GetOtherRoomsData(Rendering rendering);
        OpentimingsData GetOpenTimingsData(Rendering rendering);
        ReadMoreArticles GetOtherArticlesList(Rendering rendering);

        ShantigramData GetShantigramData(Rendering rendering);
        InfoTabsData GetInfoTabsData(Rendering rendering);
        OurPartners GetOurPartners(Rendering rendering);

    }
}