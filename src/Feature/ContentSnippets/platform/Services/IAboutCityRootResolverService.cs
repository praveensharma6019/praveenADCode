using Adani.SuperApp.Realty.Feature.ContentSnippets.Platform.Models;
using Sitecore.Data.Items;
using System.Collections.Generic;


namespace Adani.SuperApp.Realty.Feature.ContentSnippets.Platform.Services
{
    public interface IAboutCityRootResolverService
    {
        AboutCityData GetAboutCityDataList(Sitecore.Mvc.Presentation.Rendering rendering);
    }
}
