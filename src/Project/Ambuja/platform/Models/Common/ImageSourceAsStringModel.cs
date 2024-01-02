using Glass.Mapper.Sc.Configuration.Attributes;

namespace Project.AmbujaCement.Website.Models.Common
{
    public class ImageSourceAsStringModel
    {

        [SitecoreFieldAttribute(FieldId = "{3172427D-F670-49B7-9350-912294EA4073}")]
        public virtual string ImageSource { get; set; }

        [SitecoreFieldAttribute(FieldId = "{FACE4F58-F6D7-476E-A42B-ACF1096855B0}")]
        public virtual string ImageSourceMobile { get; set; }

        [SitecoreFieldAttribute(FieldId = "{49E9CD82-C887-49D3-8212-84725E1583F5}")]
        public virtual string ImageSourceTablet { get; set; }

        [SitecoreFieldAttribute(FieldId = "{7A750657-A49C-4F3A-B450-721D313BD255}")]
        public virtual string ImageAlt { get; set; }

        [SitecoreFieldAttribute(FieldId = "{7D0FE647-FDC9-421A-B222-2ED80997723A}")]
        public virtual string ImageTitle { get; set; }
    }
}