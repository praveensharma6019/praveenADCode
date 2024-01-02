using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
    public class DutyfreeProductsTextSearch : IComputedIndexField
    {
        //public string Parameters { get; set; }
        public string FieldName { get; set; }
        public string ReturnType { get; set; }

        public object ComputeFieldValue(IIndexable indexable)
        {
            string OfferFullTextSearch = string.Empty;
            Assert.ArgumentNotNull(indexable, "indexable");
            var item = (Item)(indexable as SitecoreIndexableItem);
            if (!item.TemplateID.Equals(Templates.DFCollection.ProductTemplateID))
            {
                // Log.Warn(string.Format("{0} : unsupported Template type : {1}", this, indexable.GetType()), this);
                return null;
            }

            Sitecore.Data.Database webDB = Sitecore.Configuration.Factory.GetDatabase("web");

            Item brandItem = null;
            string _brand = string.Empty;
            string _category = string.Empty;
            string _materialGroup = string.Empty;
            string _subcategory = string.Empty;
            Item brandFolder = webDB.GetItem(Templates.DFCollection.BrandFolderID.ToString());
            Item materialGroupFolder = webDB.GetItem(Templates.DFCollection.MaterialGroupFolderID.ToString());

            if (item == null)
            {
                Log.Warn(string.Format("{0} : unsupported IIndexable type : {1}", this, indexable.GetType()), this);
                return null;
            }

            #region Get Brand
            try
            {
                _brand = item.Fields[Templates.DFCollection.Brand_Name].Value;
                brandItem = webDB.GetItem(new ID(item.Fields[Templates.DFCollection.Brand_Name].Value));
            }
            catch (Exception ex)
            {
            }            
            if (brandItem != null)
            {
                _brand = brandItem.Name;
            }
            else
            {
                if (brandFolder.HasChildren)
                {
                    var brandItems = brandFolder.Children.ToList();
                    if (brandItems.Any())
                    {
                        Item brandItemSelected = brandItems.FirstOrDefault(p => p.TemplateID == Templates.DFCollection.BrandTemplateID && p.Name.ToLower().Equals(_brand.ToLower()));
                        if (brandItemSelected != null)
                        {
                            _brand = brandItemSelected.Fields[Templates.DFCollection.BrandName].ToString();
                        }
                    }
                }                
            }
            #endregion

            #region Get Catrgory and SubCategory
            
            GroupedDroplistField categoryField = item.Fields[Templates.DFCollection.Category_Name];
            GroupedDroplistField materialGroup = item.Fields[Templates.DFCollection.Material_Group_Name];
            Item categoryFolder = webDB.GetItem(materialGroupFolder.Paths.Path + "/" + materialGroup.Value);
            Item subCategoryFolder = webDB.GetItem(materialGroupFolder.Paths.Path + "/" + materialGroup.Value + "/" + categoryField.Value);
            _materialGroup = materialGroup.Value;
            if (categoryFolder.HasChildren)
            {
                var categoryItems = categoryFolder.Children.ToList();
                if (categoryItems.Any())
                {
                    Item categoryItem = categoryItems.FirstOrDefault(p => p.TemplateID == Templates.DFCollection.CategoryTemplateID && p.Name.ToLower().Equals(categoryField.Value.ToLower()));
                    if (categoryItem != null)
                    {
                        _category = categoryItem.Fields[Templates.DFCollection.Name].ToString();
                    }
                }
            }
            string subcategory = item.Fields[Templates.DFCollection.SubCategory_Name].Value;
            if (subCategoryFolder.HasChildren)
            {
                var subcategoryItems = subCategoryFolder.Children.ToList();
                if (subcategoryItems.Any())
                {
                    Item subcategoryItem = subcategoryItems.FirstOrDefault(p => p.TemplateID == Templates.DFCollection.SubCategoryTemplateID && p.Name.Equals(subcategory));
                    if (subcategoryItem != null)
                    {
                        _subcategory = subcategoryItem.Fields[Templates.DFCollection.Name].ToString();
                    }
                }
            }
            #endregion


            OfferFullTextSearch = item.Name.ToLower();
            OfferFullTextSearch +="|" + item.Fields[Templates.DFCollection.ProductName].Value.ToLower();
            OfferFullTextSearch += "|" + _brand.ToLower();
            OfferFullTextSearch += "|" + _category.ToLower();
            OfferFullTextSearch += "|" + _subcategory.ToLower();
            OfferFullTextSearch += "|" + _materialGroup.ToLower();
           

            return OfferFullTextSearch;
        }
    }
}