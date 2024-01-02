using Glass.Mapper.Sc.Configuration.Attributes;

namespace Project.AmbujaCement.Website.Models.Common
{
    public class GeneralLinkModel : LinkUrlAsStringModel
    {
        [SitecoreFieldAttribute(FieldId = "{A411F701-5EFD-4886-87AF-2A846BD18AFA}")]
        public virtual string LinkText { get; set; }

        [SitecoreFieldAttribute(FieldId = "{2E795C0F-28F1-4A3C-8C80-6665B4ABA7F7}")]
        public virtual string LinkTarget { get; set; }
    }
}