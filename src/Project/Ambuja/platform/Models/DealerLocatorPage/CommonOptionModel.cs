using Glass.Mapper.Sc.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.AmbujaCement.Website.Models.DealerLocatorPage
{
    public class CommonOptionModel
    {
        [SitecoreFieldAttribute(FieldId = "{41B342CB-E343-495C-917A-0AEB3A432281}")]
        public virtual string Id { get; set; }
        
        [SitecoreFieldAttribute(FieldId = "{20B6D9BB-ECD8-46D0-A72B-1EDC3160D342}")]
        public virtual string Label { get; set; }

        [SitecoreFieldAttribute(FieldId = "{6F9D9B45-C1D3-4624-9BB3-380E769543C4}")]
        public virtual string Type { get; set; }
    }
}