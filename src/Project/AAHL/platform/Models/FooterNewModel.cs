using Glass.Mapper.Sc.Configuration.Attributes;
using System.Collections.Generic;

namespace Project.AAHL.Website.Models
{
    [SitecoreType(TemplateId = "{6B469E4E-C07A-4469-9B42-2AB830B2ED19}", AutoMap = true)]
    public class FooterNewModel
    {
        [SitecoreChildren]
        public virtual IEnumerable<FooterDetailsList1> FooterDetails { get; set; }
    }
    [SitecoreType(TemplateId = "{2746B92A-BF5C-462B-8C68-1882497610D3}", AutoMap = true)]
    public class FooterDetailsList1
    {
        [SitecoreChildren]
        public virtual IEnumerable<MainNavigationsList1> MainNavigations { get; set; }
    }
    [SitecoreType(TemplateId= "{E050E319-6E07-4046-A295-679BEE879773}", AutoMap = true)]
    public class MainNavigationsList1
    {
        public virtual string Heading { get; set; }
        [SitecoreChildren]
        public virtual IEnumerable<MainNavigationsItemList1> Items { get; set; }
    }

    [SitecoreType(TemplateId = "{A6EC0FB2-FBE3-4984-A023-89C9A11AF454}", AutoMap = true)]
    public class MainNavigationsItemList1
    {
        public virtual  string LinkTitle { get; set; }
        public virtual string LinkUrl { get; set; }
        public virtual string Target { get; set; }
    }
  
}