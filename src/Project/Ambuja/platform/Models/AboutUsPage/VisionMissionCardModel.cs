using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using Project.AmbujaCement.Website.Models.Common;
using Project.AmbujaCement.Website.Models.GetInTouchPage;
using System.Collections.Generic;

namespace Project.AmbujaCement.Website.Models.AboutUsPage
{
    [SitecoreType(TemplateId = "{16E8C3A1-E667-4447-9635-8D7395E53EED}")]
    public class VisionMissionCardModel
    {
        [SitecoreFieldAttribute(FieldId = "{E23E26B1-6871-41CA-85A9-15FFF9BF5019}")]
        public virtual string Variant { get; set; }

        [SitecoreFieldAttribute(FieldId = "{434485EF-FE09-4440-8EF6-5E093A6723D5}")]
        public virtual string SectionID { get; set; }

        [SitecoreChildren]
        public virtual IEnumerable<VisionMissionCardDetails> Data { get; set; }
    }

    public class VisionMissionCardDetails : ImageSourceModel
    {
        [SitecoreFieldAttribute(FieldId = "{762E69E8-C338-461C-AE3C-C98ECB9E8869}")]
        public virtual string CardType { get; set; }

        [SitecoreFieldAttribute(FieldId = "{96205169-A0FC-4DF2-9732-CC576C7D2510}")]
        public virtual string MediaType { get; set; }

        //[SitecoreFieldAttribute(FieldId = "{8818B3B1-8231-46B4-B0CE-4DE0FBDA04B2}")]
        //public virtual string IconImage { get; set; }
        [SitecoreFieldAttribute(FieldId = "{710EBAA2-B458-4B1C-BAED-97B1364ED2BB}")]
        public virtual Image IconImageUrl { get; set; }
        public string IconImage
        {
            get
            {
                return IconImageUrl?.Src;
            }
        }

        [SitecoreFieldAttribute(FieldId = "{87B6565C-9872-4A36-95D3-1A069A8EE02B}")]
        public virtual string IconImageAlt { get; set; }

        [SitecoreChildren]
        public virtual IEnumerable<TextData> TextData { get; set; }
    }
}