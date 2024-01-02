using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;
using Adani.SuperApp.Realty.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Realty.Foundation.SitecoreHelper.Platform.Helper;
using Sitecore;
using Sitecore.Abstractions;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Links;
using Sitecore.Sites;

namespace Adani.SuperApp.Realty.Foundation.Search.Platform.ComputedFields
{
    public class BlogUrlComputedField : AbstractComputedIndexField
    {
        public override object ComputeFieldValue(IIndexable indexable)
        {
            string blogUrl = string.Empty;
            try
            {
                Item item = indexable as SitecoreIndexableItem;
                if (item != null)
                {
                    if (item.TemplateID.ToString().ToUpper().Equals("{2FBDA2B6-BA1B-472F-8040-3EB3059024C7}"))
                    {
                        LinkField linkField = item.Fields["link"];

                        if (linkField != null)
                        {

                            switch (linkField.LinkType)
                            {
                                case "internal":
                                case "external":
                                case "mailto":
                                case "anchor":
                                case "javascript":
                                    blogUrl = linkField.Url;
                                    break;
                                case "":
                                    break;
                                default:
                                    //logger
                                    break;
                            }

                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Log.Error("BlogUrlComputedField : ", string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));

            }
            return blogUrl;
        }
    }
}
