using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using Project.AmbujaCement.Website.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.AmbujaCement.Website.Models.ProjectDetails
{
    public class FeaturesModel
    {
        [SitecoreFieldAttribute(FieldId = "{AEF7AA36-A693-4A85-9A79-74E90ABF961C}")]
        public virtual string Heading { get; set; }

        [SitecoreChildren]
        public virtual IEnumerable<FeaturesData> Gallery { get; set; }
    }
    public class FeaturesData : ImageSourceModel
    {
        [SitecoreFieldAttribute(FieldId = "{AEF7AA36-A693-4A85-9A79-74E90ABF961C}")]
        public virtual string Heading { get; set; }

        [SitecoreFieldAttribute(FieldId = "{EA82288C-5154-4095-A205-0E89A6F2AD55}")]
        public virtual string Description { get; set; }
        [SitecoreFieldAttribute(FieldId = "{98E1B283-33D3-4D7B-9A54-6D431636491B}")]
        public virtual Image hover { get; set; }
        public string hoverImageSource
        {
            get 
            {
                return hover?.Src;
            }
        }
    }
}