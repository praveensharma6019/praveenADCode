using Adani.BAU.Transmission.Feature.Navigation.Platform.Models;
using Sitecore.Data.Items;

namespace Adani.BAU.Transmission.Feature.Navigation.Platform.Services
{
    public interface IHeaderBuilder
    {
        Header GetHeader(Item contextItem);
    }
}
