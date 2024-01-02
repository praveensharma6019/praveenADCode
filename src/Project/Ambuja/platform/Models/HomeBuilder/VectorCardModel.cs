using Glass.Mapper.Sc.Configuration.Attributes;
using Project.AmbujaCement.Website.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.AmbujaCement.Website.Models.HomeBuilder
{
    public class VectorCardModel
    {
        public virtual string readMore { get; set; }
        public virtual string readLess { get; set; }
        [SitecoreChildren]
        public virtual IEnumerable<vectorcard> data { get; set; }
    }
    public class vectorcard : ImageSourceModel
    {
        public virtual string Heading { get; set; }
        public virtual string Description { get; set; }
        public virtual string Sequence { get; set; }
    }
}