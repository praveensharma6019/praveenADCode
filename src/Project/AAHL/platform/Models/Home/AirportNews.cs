using Glass.Mapper.Sc.Configuration.Attributes;
using Project.AAHL.Website.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.AAHL.Website.Models.Home
{
    public class AirportNews
    {
        public virtual AirportNewsWidget Widget { get; set; }

    }


    public class AirportNewsWidget : WidgetModel
    {
        [SitecoreChildren]
        public virtual IEnumerable<AirportNewsWidgetItem> WidgetItems { get; set; }
    }

    public class AirportNewsWidgetItem : WidgetItemModel
    {
        [SitecoreField]
        public virtual string IconImage { get; set; }
        //[SitecoreField]
        //public virtual string Date { get; set; }
        [SitecoreFieldAttribute(FieldId = "{3123250B-0E88-4AA4-AEB7-4E1F3A1771F1}")]
        public virtual DateTime Dateformat { get; set; }
        public virtual string Date
        {
            get 
            {
                return Dateformat.ToString("MMM dd, yyyy");
            }
        }

    }
}