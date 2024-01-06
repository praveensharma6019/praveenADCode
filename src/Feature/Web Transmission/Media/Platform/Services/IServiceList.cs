using Adani.BAU.Transmission.Feature.Media.Platform.Models;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Adani.BAU.Transmission.Feature.Media.Platform.Services
{
    public interface IServiceList
    {

        services GetServicesData(Rendering rendering, string queryString);
    }
}