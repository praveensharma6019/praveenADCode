using Glass.Mapper.Sc.Configuration.Attributes;
using Project.AAHL.Website.Models.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.AAHL.Website.Models.Investors
{
    public class PerformanceModel
    {
        [SitecoreChildren]
        public virtual IEnumerable<PerformanceItem> Items { get; set; }
    }
    public class PerformanceItem : ImageModel
    {
        [SitecoreField]
        public virtual string Title { get; set; }
        [SitecoreField]
        public virtual string SubTitle { get; set; }
        [SitecoreField]
        public virtual int GridSize { get; set; }
        [SitecoreField]
        public virtual string Class { get; set; }
        [SitecoreField]
        public virtual string Direction { get; set; }
        [SitecoreField]
        public virtual string IconClass { get; set; }
        [SitecoreField]
        public virtual string Description { get; set; }
        [SitecoreField]
        public virtual string Alignment {  get; set; }
        [SitecoreFieldAttribute(FieldId = "{FCB57202-E11D-4141-83A7-16D1904581FE}")]
        public virtual widthItem Width { get; set;}

    }
    public class widthItem
    {
        [SitecoreFieldAttribute(FieldId = "{CB625B32-9C31-47B9-AC2B-7395025D1CF3}")]
        public virtual string dWidth { get; set;}
        [SitecoreFieldAttribute(FieldId = "{28A5B466-E729-473D-8503-47286E1D3E23}")]
        public virtual string mWidth { get; set;}
        [SitecoreFieldAttribute(FieldId = "{E2BA1F27-F7B4-4048-8CF0-113BB4057327}")]
        public virtual string tWidth { get; set;}
    }
}