using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using System.Collections.Generic;

namespace Project.AAHL.Website.Models.CompetetiveAdvantage
{
    public class SectionWithImagesModel
    {
        public virtual string Heading { get; set; }

        [SitecoreChildren]
        public virtual IEnumerable<SectionImagesList> Items { get; set; }
    }

    public class SectionImagesList
    {
        public virtual string Heading { get; set; }
        public virtual string Description { get; set; }
        public virtual Image ImagePath { get; set; }
        public virtual Image MImagePath { get; set; }
        public virtual Image TImagePath { get; set; }
    }
}