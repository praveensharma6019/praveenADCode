
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;

namespace Project.AAHL.Website.Models
{
    public class ImageModel
    {
        [SitecoreField]
        public virtual Image ImagePath { get; set; }
        [SitecoreField]
        public virtual Image MImagePath { get; set; }
        [SitecoreField]
        public virtual Image TImagePath { get; set; }
    }
}