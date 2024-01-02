using Adani.SuperApp.Realty.Feature.ContentSnippets.Platform.Models;
using Sitecore.Data.Items;
using System.Collections.Generic;

namespace Adani.SuperApp.Realty.Feature.ContentSnippets.Platform.Services
{
    public interface IAboutAdaniRealtyRootResolverService
    {
        AboutAdaniRealtyData GetAboutAdaniRealtyDataList(Sitecore.Mvc.Presentation.Rendering rendering);
    }
}
