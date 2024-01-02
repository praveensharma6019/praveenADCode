using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using Project.AmbujaCement.Website.Models.Common;
using System.Collections.Generic;
using Link = Glass.Mapper.Sc.Fields.Link;

namespace Project.AmbujaCement.Website.Models.AboutUsPage
{
    public class ICanCardsModel
    {

        [SitecoreFieldAttribute(FieldId = "{AEF7AA36-A693-4A85-9A79-74E90ABF961C}")]
        public virtual string Heading { get; set; }

        [SitecoreFieldAttribute(FieldId = "{782B0141-674E-41E2-BA9C-641E26B56957}")]
        public virtual string Theme { get; set; }

        public virtual string Variant { get; set; }

        [SitecoreFieldAttribute(FieldId = "{434485EF-FE09-4440-8EF6-5E093A6723D5}")]
        public virtual string SectionID { get; set; }

        [SitecoreChildren]
        public virtual IEnumerable<GalleryItem> Gallery { get; set; }
    }


    public class GalleryItem : ImageSourceModel //: ImageSourceAsStringModel (For Image as a String)
    {

        //[SitecoreFieldAttribute(FieldId = "{C5E8AC99-8573-4BCE-907C-501707EAB25B}")]
        //public virtual string Link { get; set; }
        [SitecoreFieldAttribute(FieldId = "{7BE690ED-01D2-4F6B-8A18-7C2D58AD2AFF}")]
        public virtual Link LinkUrl { get; set; }
        public string Link
        {
            get
            {
                return LinkUrl?.Url;
            }

        }

        [SitecoreFieldAttribute(FieldId = "{A411F701-5EFD-4886-87AF-2A846BD18AFA}")]
        public virtual string LinkText { get; set; }

        [SitecoreFieldAttribute(FieldId = "{2E795C0F-28F1-4A3C-8C80-6665B4ABA7F7}")]
        public virtual string LinkTarget { get; set; }

        [SitecoreFieldAttribute(FieldId = "{AEF7AA36-A693-4A85-9A79-74E90ABF961C}")]
        public virtual string Heading { get; set; }

        [SitecoreFieldAttribute(FieldId = "{EA82288C-5154-4095-A205-0E89A6F2AD55}")]
        public virtual string Description { get; set; }

        [SitecoreFieldAttribute(FieldId = "{5375E0A2-0FD8-4BBC-8A2A-D3FFC276FD43}")]
        public virtual string Badge { get; set; }

        [SitecoreFieldAttribute(FieldId = "{1C468078-12C4-41CF-BF89-5728AE352962}")]
        public virtual GtmDetails gtmData { get; set; }
    }
}