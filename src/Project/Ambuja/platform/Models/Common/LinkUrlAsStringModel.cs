using Glass.Mapper.Sc.Configuration.Attributes;

namespace Project.AmbujaCement.Website.Models.Common
{
    public class LinkUrlAsStringModel
    {
        [SitecoreFieldAttribute(FieldId = "{C5E8AC99-8573-4BCE-907C-501707EAB25B}")]
        public virtual string Link { get; set; }
    }
}