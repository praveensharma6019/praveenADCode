using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using Project.AmbujaCement.Website.Models.Common;
using System.Collections.Generic;
using Link = Glass.Mapper.Sc.Fields.Link;

namespace Project.AmbujaCement.Website.Models.HomeBuilder
{
    public class BisStandardsModel
    {
        [SitecoreChildren]
        public virtual IEnumerable<BisStandardList> standards { get; set; }
    }
    public class BisStandardList
    {
        [SitecoreFieldAttribute(FieldId = "{5D016A88-C7B3-4D31-B167-304FB6D50593}")]
        public virtual string Name { get; set; }
        [SitecoreFieldAttribute(FieldId = "{5B153ED3-8466-4725-874F-CE56E5C5C629}")]
        public virtual string Standard { get; set; }
    }
}