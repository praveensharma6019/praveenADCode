using Adani.SuperApp.Airport.Feature.MetaData.Platform.Models;
using Sitecore.Data.Items;

namespace Adani.SuperApp.Airport.Feature.MetaData.Platform.Services
{
    public interface IPageMetaData
    {
      
        Metadata GetMetadata(Item datasource);
    }
}
