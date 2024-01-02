using System;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;

namespace Adani.SuperApp.Airport.Foundation.Search.Platform.ComputedFields
{
    public class DutyfreeBrand : IComputedIndexField
    {
        //public string Parameters { get; set; }
        public string FieldName { get; set; }
        public string ReturnType { get; set; }

        public object ComputeFieldValue(IIndexable indexable)
        {
            Assert.ArgumentNotNull(indexable, "indexable");
            var item = (Item)(indexable as SitecoreIndexableItem);

            if (item == null)
            {
                Log.Warn(string.Format("{0} : unsupported IIndexable type : {1}", this, indexable.GetType()), this);
                return null;
            }

            if (!item.TemplateID.Equals(Templates.DFCollection.ProductTemplateID))
            {
                return null;
            }
            Sitecore.Data.Database webDB = Sitecore.Configuration.Factory.GetDatabase("web");
            Item brandItem = null;
            try
            {
                brandItem = webDB.GetItem(new ID(item.Fields[Templates.DFCollection.Brand_Name].Value));
            }
            catch (Exception ex)
            {
            }
            string _brand = string.Empty;
            if(brandItem != null)
            {
                _brand = brandItem.Name;
            }
            else
            {
                _brand = item.Fields[Templates.DFCollection.Brand_Name].Value;
            }

            return _brand.ToLower();
        }
    }
}