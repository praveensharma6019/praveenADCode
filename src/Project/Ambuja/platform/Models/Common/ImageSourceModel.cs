using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;

namespace Project.AmbujaCement.Website.Models.Common
{
    public class ImageSourceModel
    {
        [SitecoreFieldAttribute(FieldId = "{21E8E999-45DF-4739-9625-CB21E9B7495C}")]
        public virtual Image ImageUrl { get; set; }
        public string ImageSource
        {
            get
            {
                return ImageUrl?.Src;
            }
        }

        [SitecoreFieldAttribute(FieldId = "{2016699D-DF17-47B0-96A7-38C3EA64F70E}")]
        public virtual Image ImageMobileUrl { get; set; }
        public string ImageSourceMobile
        {
            get
            {
                return ImageMobileUrl?.Src;
            }
        }

        [SitecoreFieldAttribute(FieldId = "{D2F68ABF-76C6-4E8C-BEC5-BEB468E3371C}")]
        public virtual Image ImageTabletUrl { get; set; }
        public string ImageSourceTablet
        {
            get
            {
                return ImageTabletUrl?.Src;
            }
        }

        [SitecoreFieldAttribute(FieldId = "{FB714145-72C6-4775-AFC8-06C645272455}")]
        public virtual string ImageAlt { get; set; }

        [SitecoreFieldAttribute(FieldId = "{E5141247-CE3D-4A2F-AC25-228A9286A796}")]
        public virtual string ImageTitle { get; set; }
    }
}