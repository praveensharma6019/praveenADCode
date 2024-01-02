using Adani.SuperApp.Realty.Feature.ContentSnippets.Platform.Models;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System.Collections.Generic;


namespace Adani.SuperApp.Realty.Feature.ContentSnippets.Platform.Services
{
    public interface IClubDetailsRootResolverService
    {
        ClubHeroBannerData GetClubHeroBannerDataList(Rendering rendering);

        AboutAdaniSocialClubData GetAboutAdaniSocialClubDataList(Rendering rendering);

        ClubHighLightsData GetClubHighLightsDataList(Rendering rendering);

        AboutClubData GetAboutClubDataList(Rendering rendering);       
    }
}
