using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using Microsoft.OData.Edm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.AAHL.Website.Models.OurStory
{
    public class OurTimelineCarousel
    {
        [SitecoreField]
        public virtual string Heading { get; set; }
        [SitecoreField]
        public virtual string Description { get; set; }
        [SitecoreField]
        public virtual string Theme { get; set; }
        [SitecoreChildren]
        public virtual IEnumerable<OurTimelineCarouselItem> Items { get; set; }
    }

    public class OurTimelineCarouselItem
    {
        [SitecoreChildren]
        public virtual IEnumerable<OurTimelineCarouseSubitem> subitems { get; set; }
        [SitecoreField]
        public virtual string Title { get; set; }
        [SitecoreField]
        public virtual Link LinkUrl { get; set; }
        [SitecoreField]
        public virtual bool Isactive { get; set; }
    }

    public class OurTimelineCarouseSubitem : ImageModel
    {
        [SitecoreField]
        public virtual string Heading { get; set; }
        [SitecoreField]
        public virtual string Description { get; set; }
        [SitecoreField]
        public virtual bool Isactive { get; set; }
        //[SitecoreField]
        //public virtual string Date { get; set; }
        [SitecoreFieldAttribute(FieldId = "{3123250B-0E88-4AA4-AEB7-4E1F3A1771F1}")]
        public virtual DateTime Dateformat { get; set; }
        public virtual string Date
        {
            get
            {
                return Dateformat.ToString("dd MMM yyyy");
            }
        }
    }
}