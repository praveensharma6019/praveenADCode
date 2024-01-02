using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using System.Collections.Generic;

namespace Project.AAHL.Website.Models.OurStrategy
{
    public class CardDetailWithImagesModel
    {
        public virtual string Heading { get; set; }
        public virtual string Description { get; set; }
        [SitecoreChildren]
        public virtual IEnumerable<CardImageList> Items { get; set; }
    }
    public class CardImageList
    {
        public virtual Image ImagePath { get; set; }
        public virtual Image MImagePath { get; set; }
        public virtual Image TImagePath { get; set; }
    }
}