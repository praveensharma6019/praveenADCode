using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using System;
using System.Linq;

namespace Adani.SuperApp.Airport.Foundation.Search.Platform.ComputedFields
{
    public class DutyfreeSubCategoryTitle : IComputedIndexField
    {
        //public string Parameters { get; set; }
        public string FieldName { get; set; }
        public string ReturnType { get; set; }

        public object ComputeFieldValue(IIndexable indexable)
        {
            string subcategoryTitle = string.Empty;
            Assert.ArgumentNotNull(indexable, "indexable");
            var item = (Item)(indexable as SitecoreIndexableItem);

            if (item == null)
            {
                Log.Warn(string.Format("{0} : unsupported IIndexable type : {1}", this, indexable.GetType()), this);
                return null;
            }

            if (!item.TemplateID.Equals(Templates.DFCollection.ProductTemplateID))
            {
             //   Log.Warn(string.Format("{0} : unsupported Template type : {1}", this, indexable.GetType()), this);
                return null;
            }
            try
            {
                Sitecore.Data.Database webDB = Sitecore.Configuration.Factory.GetDatabase("web");
                Item materialGroupFolder = webDB.GetItem(Templates.DFCollection.MaterialGroupFolderID.ToString());
                string subcategory = item.Fields[Templates.DFCollection.SubCategory_Name].Value;
                GroupedDroplistField materialGroup = item.Fields[Templates.DFCollection.Material_Group_Name];
                GroupedDroplistField category = item.Fields[Templates.DFCollection.Category_Name];
                Item categoryFolder = webDB.GetItem(materialGroupFolder.Paths.Path + "/" + materialGroup.Value + "/" + category.Value);

                if (categoryFolder.HasChildren)
                {
                    var categoryItems = categoryFolder.Children.ToList();
                    if (categoryItems.Any())
                    {
                        Item subcategoryItem = categoryItems.FirstOrDefault(p=> p.TemplateID == Templates.DFCollection.SubCategoryTemplateID && p.Name.Equals(subcategory)) ;
                        if(subcategoryItem != null)
                        {
                            subcategoryTitle = subcategoryItem.Fields[Templates.DFCollection.Name].ToString();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Log.Warn(string.Format("{0} : unsupported Template type : {1} -- Error -- {2}", this, indexable.GetType(), ex), this);
            }


            return subcategoryTitle;
        }
    }
}