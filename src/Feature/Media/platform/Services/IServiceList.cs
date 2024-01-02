using Adani.SuperApp.Airport.Feature.Media.Platform.Models;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Adani.SuperApp.Airport.Feature.Media.Platform.Services
{
    public interface IServiceList
    {

        services GetServicesData(Rendering rendering, string queryString);
    }
}