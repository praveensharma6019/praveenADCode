using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Mining.Website.Models.Common
{
    public class SEO
    {
        public virtual string PageName { get; set; }
        public virtual string Robots { get; set; }
        public virtual Link CanonicalURL { get; set; }

        //MetaTags
        public virtual string Title { get; set; }
        public virtual string MetaDescription { get; set; }
        public virtual string Keywords { get; set; }
        public virtual string Ogtitle { get; set; }
        public virtual string Ogdescription { get; set; }
        public virtual string Ogkeywords { get; set; }
        public virtual Image Ogimage { get; set; }

        //SchemaTags
        public virtual string Context { get; set; }
        public virtual string Type { get; set; }
        public virtual string Name { get; set; }
        public virtual string LogoType { get; set; }
        public virtual Link LogoURL { get; set; }
        public virtual string Width { get; set; }
        public virtual string Height { get; set; }
        public virtual Link PageURL { get; set; }

    }
}