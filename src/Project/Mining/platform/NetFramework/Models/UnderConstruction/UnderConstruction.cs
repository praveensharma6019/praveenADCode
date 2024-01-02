using Glass.Mapper.Sc.Fields;
using Project.Mining.Website.Models.Common;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Mining.Website.Models.UnderConstruction
{
    public class UnderConstruction : ImageModel
    {
        public virtual string Heading { get; set; }
        public virtual string Description { get; set; }
        public virtual Image AnimationImage { get; set; }
    }
}