using Adani.SuperApp.Realty.Feature.ContentSnippets.Platform.Models;
using Sitecore.Data.Items;
using System.Collections.Generic;


namespace Adani.SuperApp.Realty.Feature.ContentSnippets.Platform.Services
{
    public interface IAboutGoodLifeRootResolverService
    {
        AboutGoodLifeData GetAboutGoodLife(Sitecore.Mvc.Presentation.Rendering rendering);
        AOGBriefModel GetAOGBrief(Sitecore.Mvc.Presentation.Rendering rendering);
    }
}
