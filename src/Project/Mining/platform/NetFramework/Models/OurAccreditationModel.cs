using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using Project.Mining.Website.Models.Common;
using System.Collections.Generic;

namespace Project.Mining.Website.Models
{
    public class OurAccreditationModel
    {       
        public virtual Link CTALink { get; set; }
        public virtual string Description { get; set; }
        public virtual string SubHeading { get; set; }
        public virtual string Heading { get; set; }
        public virtual string Class { get; set; }
        [SitecoreChildren]
        public virtual IEnumerable<OurAccreditationlist> LogoImages { get; set; }

        public class OurAccreditationlist:ImageModel
        {
            [SitecoreField]
            public virtual Link CTALink { get; set; }
            [SitecoreField]
            public virtual string LinkIcon { get; set; }

        }
       
     }
}