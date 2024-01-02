using Sitecore.Data.Fields;
using Sitecore.Data.Items;

namespace Adani.SuperApp.Airport.Feature.MetaData.Platform.Models
{
    public static class SitecoreItemExtensions
    {
        public static string GetDropLinkValue(Field fieldName)
        {
            GroupedDroplinkField buttonVariant = fieldName;
            if (buttonVariant != null && buttonVariant.TargetItem != null)
            {
                return buttonVariant.TargetItem.Fields["Value"].Value;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}