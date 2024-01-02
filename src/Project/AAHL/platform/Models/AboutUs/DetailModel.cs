using Glass.Mapper.Sc.Configuration.Attributes;
using System.Collections.Generic;

namespace Project.AAHL.Website.Models.AboutUs
{
    //[SitecoreType(TemplateId = "{6B469E4E-C07A-4469-9B42-2AB830B2ED19}", AutoMap = true)]
    public class DetailModel
    {
        [SitecoreField]
        public virtual string Heading { get; set; }
        [SitecoreField]
        public virtual string Description { get; set; }
    }
}