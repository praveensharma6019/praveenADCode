using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using System.Collections.Generic;
using Link = Glass.Mapper.Sc.Fields.Link;

namespace Project.AmbujaCement.Website.Models.AboutUsPage
{
    public class AchievementsModel
    {
        [SitecoreChildren]
        public virtual IEnumerable<Achievement> data { get; set; }
    }

    [SitecoreType(TemplateId = "{EC78279C-6150-47E8-99D2-9900D4BC5945}")]
    public class Achievement
    {
        //[SitecoreFieldAttribute(FieldId = "{6242B56C-10A0-46CE-8044-C41AB874941A}")]
        //public virtual string icon { get; set; }
        [SitecoreFieldAttribute(FieldId = "{8B3E60A5-FD41-4A37-BBFD-53F983FA4082}")]
        public virtual Image IconUrl { get; set; }
        public string icon
        {
            get
            {
                return IconUrl?.Src;
            }

        }

        [SitecoreFieldAttribute(FieldId = "{07340B0F-8A84-4201-A21C-7FB7F0547518}")]
        public virtual string start { get; set; }

        [SitecoreFieldAttribute(FieldId = "{09D523CA-580A-436E-B611-69EB967D228F}")]
        public virtual string count { get; set; }

        [SitecoreFieldAttribute(FieldId = "{71D82FFD-04B5-4D84-8DE7-45494AFDA581}")]
        public virtual string delay { get; set; }

        [SitecoreFieldAttribute(FieldId = "{63AC5D8B-BD16-4549-8556-9AFC601C2F98}")]
        public virtual string desc { get; set; }
    }
}