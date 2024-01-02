using Adani.SuperApp.Realty.Feature.Configuration.Platform.Models;
using Sitecore.Mvc.Presentation;

namespace Adani.SuperApp.Realty.Feature.Configuration.Platform.Services
{
    public interface ICommonDataService
    {
        CommonDataModel GetCommonData(Rendering rendering);
        CommonText GetCommonText(Rendering rendering);

    }
}