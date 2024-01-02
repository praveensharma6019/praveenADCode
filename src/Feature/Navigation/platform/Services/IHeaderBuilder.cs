using Adani.SuperApp.Airport.Feature.Navigation.Platform.Models;
using Sitecore.Data.Items;

namespace Adani.SuperApp.Airport.Feature.Navigation.Platform.Services
{
    public interface IHeaderBuilder
    {
        Header GetHeader(Item contextItem);
    }
}
