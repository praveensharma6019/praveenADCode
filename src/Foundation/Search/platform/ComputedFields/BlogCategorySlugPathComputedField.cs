using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using System;
using System.Reflection;

namespace Adani.SuperApp.Realty.Foundation.Search.Platform.ComputedFields
{
    public class BlogCategorySlugPathComputedField : AbstractComputedIndexField
    {
        public override object ComputeFieldValue(IIndexable indexable)
        {
            string slugPath = string.Empty;
            try
            {
                Item item = indexable as SitecoreIndexableItem;
                if (item != null)
                {
                    if (item.TemplateID.ToString().ToUpper().Equals("{2FBDA2B6-BA1B-472F-8040-3EB3059024C7}"))
                    {
                        ReferenceField dropLinkField = item?.Fields["Category Title"];

                        if (dropLinkField != null && dropLinkField?.TargetItem != null)
                        {
                            Item categoryItem = dropLinkField.TargetItem;
                            slugPath = categoryItem.Paths.FullPath;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("BlogCategorySlugPathComputedField : ", string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return slugPath;
        }
    }
}