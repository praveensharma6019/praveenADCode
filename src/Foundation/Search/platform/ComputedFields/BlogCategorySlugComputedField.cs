using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Adani.SuperApp.Realty.Foundation.Search.Platform.ComputedFields
{
    public class BlogCategorySlugComputedField : AbstractComputedIndexField
    {
        public override object ComputeFieldValue(IIndexable indexable)
        {
            string slugText = string.Empty;
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
                            slugText =  categoryItem?.Fields["Slug Text"]?.Value;

                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("BlogCategorySlugComputedField : ",string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return slugText;
        }
    }
}