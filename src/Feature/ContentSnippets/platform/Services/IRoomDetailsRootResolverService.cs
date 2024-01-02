using Adani.SuperApp.Realty.Feature.ContentSnippets.Platform.Models;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System.Collections.Generic;


namespace Adani.SuperApp.Realty.Feature.ContentSnippets.Platform.Services
{
    public interface IRoomDetailsRootResolverService
    {
        RoomTitleData GetRoomTitleDataList(Rendering rendering);

        RoomInfoTabsData GetRoomInfoTabsDataList(Rendering rendering);

        MostFacilitiesData GeMostFacilitiesDataList(Rendering rendering);

        FacilitiesCategoriesData GetFacilitiesCategoriesDataList(Rendering rendering);

        otherRoomsData GetOtherRoomsData(Rendering rendering);
    }
}
