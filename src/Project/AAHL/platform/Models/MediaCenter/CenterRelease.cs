using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Web;

namespace Project.AAHL.Website.Models.MediaCenter
{
    public class CenterRelease
    {
        [SitecoreChildren]
        public virtual IEnumerable<MediaItem> Items { get; set; }
    }
    public class MediaItem
    {
        [SitecoreField]
        public virtual string Heading { get; set; }
        [SitecoreField]
        public virtual string Description { get; set; }
        [SitecoreField]
        public virtual bool Isactive { get; set; }
        [SitecoreField]
        public virtual Link LinkUrl { get; set; }
        [SitecoreField("Date")]
        public virtual IEnumerable<CategoriesChilditems> Date { get; set; }
        [SitecoreChildren]
        public virtual IEnumerable<MediaReleaseChilditems> Childitems { get; set; }
        [SitecoreField]
        public virtual string BtnText { get; set; }
    }
    public class MediaReleaseChilditems : ImageModel
    {
        [SitecoreField]
        public virtual string Description { get; set; }
        [SitecoreField]
        public virtual bool Isvideo { get; set; }
        [SitecoreField]
        public virtual Link LinkUrl { get; set; }
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
        [SitecoreField]
        public virtual string Year { get; set; }
        [SitecoreField]
        public virtual bool IsYear { get; set; }
        [SitecoreField]
        public virtual string IconImage { get; set; }
        [SitecoreField]
        public virtual Link VideoSource { get; set; }
    }
}