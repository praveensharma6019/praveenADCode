using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using Project.Mining.Website.Models.Common;
using System.Collections.Generic;


namespace Project.Mining.Website.Models
{
    public class OurServicesModel : SectionHeadingModel
    {
        public virtual string LeftArrowIcon { get; set; }
        public virtual string RightArrowIcon { get; set; }
        public virtual Link Link { get; set; }
        [SitecoreChildren]
        public virtual IEnumerable<OurServicesdatalist> OurServicesdata { get; set; }

        public class OurServicesdatalist : ImageModel
        {
            public virtual string Description { get; set; }
            public virtual Link Link { get; set; }
            public virtual bool IsSelected { get; set; }
            public virtual string BgColor { get; set; }
        }
    }
}