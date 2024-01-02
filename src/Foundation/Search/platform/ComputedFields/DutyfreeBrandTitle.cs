using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
namespace Adani.SuperApp.Airport.Foundation.Search.Platform.ComputedFields
{
    public class DutyfreeBrandTitle : IComputedIndexField
    {
        //public string Parameters { get; set; }
        public string FieldName { get; set; }
        public string ReturnType { get; set; }

        public object ComputeFieldValue(IIndexable indexable)
        {
            string brandTitle = string.Empty;
            Assert.ArgumentNotNull(indexable, "indexable");
            var item = (Item)(indexable as SitecoreIndexableItem);

            if (item == null)
            {
                Log.Warn(string.Format("{0} : unsupported IIndexable type : {1}", this, indexable.GetType()), this);
                return null;
            }

            if (!item.TemplateID.Equals(Templates.DFCollection.ProductTemplateID))
            {
               // Log.Warn(string.Format("{0} : unsupported Template type : {1}", this, indexable.GetType()), this);
                return null;
            }
            Sitecore.Data.Database webDB = Sitecore.Configuration.Factory.GetDatabase("web");
            Item brandFolder = webDB.GetItem(Templates.DFCollection.BrandFolderID.ToString());
            Item brandItem = null;
            try
            {
                brandItem = webDB.GetItem(new ID(item.Fields[Templates.DFCollection.Brand_Name].Value));
            }
            catch (Exception ex)
            {
            }
            string _brand = string.Empty;
            if (brandItem != null)
            {
                _brand = brandItem.Name;
            }
            else
            {
                _brand = item.Fields[Templates.DFCollection.Brand_Name].Value;
            }
            if (brandFolder.HasChildren)
            {
                var brandItems = brandFolder.Children.ToList();
                if (brandItems.Any())
                {
                   Item brandItemSelected = brandItems.FirstOrDefault(p => p.TemplateID == Templates.DFCollection.BrandTemplateID && p.Name.ToLower().Equals(_brand.ToLower()));
                    if (brandItemSelected != null)
                    {
                        brandTitle = brandItemSelected.Fields[Templates.DFCollection.BrandName].ToString();
                    }
                }
            }

            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(brandTitle.ToLower());
        }
    }
}