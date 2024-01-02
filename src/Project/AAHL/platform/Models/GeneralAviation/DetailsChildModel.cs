using Glass.Mapper.Sc.Configuration.Attributes;
using System.Collections.Generic;

namespace Project.AAHL.Website.Models.GeneralAviation
{
    public class DetailsChildModel
    {
        [SitecoreField]
        public virtual string Heading { get; set; }
        [SitecoreField]
        public virtual string Description { get; set; }
        [SitecoreChildren]
        public virtual IEnumerable<ImageChild> Items { get; set; }
    }
    public class ImageChild : ImageModel
    {

    }
}