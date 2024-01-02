using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Resources.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Foundation.Search.Platform.ComputedFields
{
    public class AirlineTypeComputedField : IComputedIndexField
    {
        //public string Parameters { get; set; }
        public string FieldName { get; set; }
        public string ReturnType { get; set; }
        //string image;

        public object ComputeFieldValue(IIndexable indexable)
        {
            Assert.ArgumentNotNull(indexable, "indexable");
            var item = (Item)(indexable as SitecoreIndexableItem);

            if (item == null)
            {
                Log.Warn(string.Format("{0} : unsupported IIndexable type : {1}", this, indexable.GetType()), this);
                return null;
            }

            if (!item.TemplateID.Equals(Templates.AirlineCollection.AirlineTemplateID))
            {
                Log.Warn(string.Format("{0} : unsupported Template type : {1}", this, indexable.GetType()), this);
                return null;
            }

            GroupedDroplistField droplistField = item.Fields[Templates.AirlineCollection.AirlineType];

            return (droplistField == null || droplistField.Value == null) ? null : droplistField.Value;
        }
    }
}