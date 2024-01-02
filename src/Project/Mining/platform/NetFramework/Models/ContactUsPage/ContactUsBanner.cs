using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using Project.Mining.Website.Models.Common;
using System.Collections.Generic;

namespace Project.Mining.Website.Models.ContactUsPage
{
    public class ContactUsBanner: ImageModel
    {
        public virtual string Heading { get; set; }
        [SitecoreChildren]
        public virtual IEnumerable<ContactUsBannerData> BannerItems { get; set; }
    }
    public class ContactUsBannerData : ImageModel
    {
        [SitecoreField]
        public virtual Link Link { get; set; }
        [SitecoreFieldAttribute(FieldId = "{838EFDFA-8538-4768-A508-37627E7D7F49}")]
        public virtual GtmDataModel GtmData { get; set; }
    }
}