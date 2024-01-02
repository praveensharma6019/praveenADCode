using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using Project.Mining.Website.Models.Common;
using System.Collections.Generic;
using static Project.Mining.Website.Models.WhoWeAreModel;

namespace Project.Mining.Website.Models
{
    public class TermsandConditions
    {
        public virtual string Description { get; set; }
        public virtual string Heading { get; set; }
    }
}