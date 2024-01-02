using Adani.SuperApp.Realty.Feature.ContentSnippets.Platform.Models;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System.Collections.Generic;


namespace Adani.SuperApp.Realty.Feature.ContentSnippets.Platform.Services
{
    public interface IOrderConfirmationRootResolverService
    {
        ConfirmBannerData GetConfirmBannerDataList(Rendering rendering);

        OrderDetailsData GetOrderDetailsDataList(Rendering rendering);

        SaveDetailsData GetSaveDetailsDataList(Rendering rendering);

        ExploreData GetExploreData(Rendering rendering);

        ConfigurationData GetConfigurationData(Rendering rendering);
    }
}
