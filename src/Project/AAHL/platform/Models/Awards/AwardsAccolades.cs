using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.AAHL.Website.Models.Awards
{

    public class AwardsAccolades
    {
        [SitecoreChildren]
        public virtual IEnumerable<AwardsAccoladesItem> Items { get; set; }
    }

    public class AwardsAccoladesItem
    {
        public virtual string Heading { get; set; }
        public virtual string Subheading { get; set; }
        public virtual string Description { get; set; }
        public virtual bool Isactive { get; set; }
        public virtual Link LinkUrl { get; set; }
        [SitecoreChildren]
        public virtual IEnumerable<AwardList> subItems { get; set; }
    }
    public class AwardList : ImageModel
    {
        //public virtual string Date { get; set; }
        public virtual bool IsYear { get; set; }
        [SitecoreFieldAttribute(FieldId = "{3123250B-0E88-4AA4-AEB7-4E1F3A1771F1}")]
        public virtual DateTime Dateformat { get; set; }
        public virtual string Date
        {
            get
            {
                return Dateformat.ToString("MMM dd, yyyy");
            }
        }
        public virtual string Subheading { get; set; }
    }
}