using Glass.Mapper.Sc.Fields;
using Project.Mining.Website.Models.Common;


namespace Project.Mining.Website.Models
{
    public class WhyMtcsModel : ImageModel
    {
            public virtual Link CTALink { get; set; }
            public virtual string Class { get; set; }
            public virtual string Description { get; set; }
            public virtual string SubHeading { get; set; }
            public virtual string Heading { get; set; }
       
     }
}