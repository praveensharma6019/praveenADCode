using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using System.Linq;

namespace Adani.SuperApp.Airport.Foundation.Search.Platform.ComputedFields
{
    public class DutyfreeMaterialGroupTitle : IComputedIndexField
    {
        //public string Parameters { get; set; }
        public string FieldName { get; set; }
        public string ReturnType { get; set; }

        public object ComputeFieldValue(IIndexable indexable)
        {
            string materialGroupTitle = string.Empty;
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
            Sitecore.Data.Database webDB = Sitecore.Configuration.Factory.GetDatabase("web");
            Item materialGroupFolder = webDB.GetItem(Templates.DFCollection.MaterialGroupFolderID.ToString());           
            GroupedDroplistField droplistField = item.Fields[Templates.DFCollection.Material_Group_Name];
            
            if (materialGroupFolder.HasChildren)
            {
                var materialGroupItems = materialGroupFolder.Children.ToList();
                if (materialGroupItems.Any())
                {
                    Item materialGroupTitleItem = materialGroupItems.FirstOrDefault(p => p.TemplateID == Templates.DFCollection.MaterialGroupTemplateID && p.Name.ToLower().Equals(droplistField.Value.ToLower()));
                    if (materialGroupTitleItem != null)
                    {
                        materialGroupTitle = materialGroupTitleItem.Fields[Templates.DFCollection.Title].ToString();
                    }

                }
            }

            return materialGroupTitle;
        }
    }
}