using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using Project.Mining.Website.Models.Common;

namespace Project.Mining.Website.Models
{
    public class DiscoverMiningBrochureModel : ImageModel
    {

        public virtual string Description { get; set; }

        [SitecoreFieldAttribute(FieldId = "{DB2322B4-1437-4BA5-B116-83B649AE6656}")]
        public virtual string Heading { get; set; }

        [SitecoreFieldAttribute(FieldId = "{AFCF1024-BBB1-4BA0-83E9-D7F1D168350E}")]
        public virtual string SubHeading { get; set; }

        public virtual Link CTALink { get; set; }
        public virtual string CTAText { get; set; }
        public virtual Image CtaImage { get; set; }
        public virtual string Class { get; set; }

    }
}