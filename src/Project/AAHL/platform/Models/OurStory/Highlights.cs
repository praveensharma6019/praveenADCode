using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.AAHL.Website.Models.OurStory
{
    public class Highlights
    {
        [SitecoreField]
        public virtual string Heading { get; set; }
        [SitecoreChildren]
        public virtual IEnumerable<HighlightstItem> Items { get; set; }
    }

    public class HighlightstItem
    {
        [SitecoreChildren]
        public virtual IEnumerable<HighlightsSubitem> subitems { get; set; }
        [SitecoreField]
        public virtual string Heading { get; set; }
        [SitecoreField]
        public virtual Link LinkUrl { get; set; }
        [SitecoreField]
        public virtual bool Isactive { get; set; }
    }

    public class HighlightsSubitem
    {
        [SitecoreField]
        public virtual string Title { get; set; }
        [SitecoreField]
        public virtual string Subtitle { get; set; }
        [SitecoreField]
        public virtual string IconImage { get; set; }
        [SitecoreField]
        public virtual string Class { get; set; }
    }
}