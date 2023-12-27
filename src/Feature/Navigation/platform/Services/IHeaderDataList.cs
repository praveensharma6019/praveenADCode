using Adani.SuperApp.Airport.Feature.Navigation.Platform.Models;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Adani.SuperApp.Airport.Feature.Navigation.Platform.Services
{
    public interface IHeaderDataList
    {
        HeaderData GetHeaderData(Item datasource);
    }
}
