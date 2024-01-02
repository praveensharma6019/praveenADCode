using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using Project.Mining.Website.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Mining.Website.Models.PageNotFound
{
    public class PageNotFound : ImageModel
    {
        [SitecoreField]
        public virtual Link CTALink { get; set; }
        [SitecoreField]
        public virtual string CTAText { get; set; }
        [SitecoreField]
        public virtual string Heading { get; set; }
        [SitecoreField]
        public virtual string Description { get; set; }

        [SitecoreFieldAttribute(FieldId = "{5064333C-E7DC-44F3-9134-B875B93F8973}")]
        public virtual string Category { get; set; }

        [SitecoreFieldAttribute(FieldId = "{CEC794D8-812B-45D7-B841-ADBEF3B1437C}")]
        public virtual string Sub_category { get; set; }

        [SitecoreFieldAttribute(FieldId = "{B2B748B1-F72D-4EB9-9B79-83D533C63E74}")]
        public virtual string Page_type { get; set; }

        [SitecoreFieldAttribute(FieldId = "{82483181-3659-43BD-9360-451AA6389EC2}")]
        public virtual string Banner_Category { get; set; }

        [SitecoreFieldAttribute(FieldId = "{35FF67F8-2A14-44DC-93CC-D3DA3F57CA75}")]
        public virtual string Label { get; set; }

        [SitecoreFieldAttribute(FieldId = "{82464190-D784-46F0-9440-63DCBC91A6F9}")]
        public virtual string Index { get; set; }

        [SitecoreFieldAttribute(FieldId = "{33F90480-C256-4EAC-83E6-2A1CF66AF185}")]
        public virtual string Event { get; set; }

    }
}