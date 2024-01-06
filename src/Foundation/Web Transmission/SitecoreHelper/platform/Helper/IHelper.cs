using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;

namespace Adani.BAU.Transmission.Foundation.SitecoreHelper.Platform.Helper
{
    public interface IHelper
    {
        // IEnumerable<Item> GetMultiListValueItems(this Item item, ID fieldId);
        string GetImageURL(Item item, string fieldName);
        string GetUrlDomain();
        string GetImageAlt(Item item, string fieldName);
        string GetLinkURL(Item item, string fieldName);
        string GetProductItemUrl(Item Item);
        string GetImageUrlfromSitecore(string path);
        string GetLinkText(Item item, string fieldName);
        string GetImageURLByFieldId(Item item, string fieldId);
        bool GetCheckboxOption(Field field);
        string GetDropLinkValue(Field fieldName);
        string GetDropListValue(Field fieldName);
        string Sanitize(string name);
        string GetImageURLbyField(Field field);
        string GetImageAltbyField(Field field);
        string GetLinkURLbyField(Item item, Field field);
        string GetLinkTextbyField(Item item, Field field);
        string ToTitleCase(string title);
        String LinkUrl(Sitecore.Data.Fields.LinkField lf);
        String LinkUrlText(Sitecore.Data.Fields.LinkField lf);
        String LinkUrlStyleclass(Sitecore.Data.Fields.LinkField lf);
        string GetImageURL(Sitecore.Data.Fields.ImageField imageField);

        void LoadAssembly();


    }
}
