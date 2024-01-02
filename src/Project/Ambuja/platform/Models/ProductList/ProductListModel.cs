using Castle.Components.DictionaryAdapter;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using Project.AmbujaCement.Website.Models.Common;
using Sitecore.Xml.Xsl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Link = Glass.Mapper.Sc.Fields.Link;

namespace Project.AmbujaCement.Website.Models.ProductList
{
    public class ProductListModel : ImageSourceModel
    {
        [SitecoreFieldAttribute(FieldId = "{434485EF-FE09-4440-8EF6-5E093A6723D5}")] 
        public virtual string SectionID { get; set; }

        [SitecoreFieldAttribute(FieldId = "{1C468078-12C4-41CF-BF89-5728AE352962}")]
        public virtual GtmDetails GtmData { get; set; }

        [SitecoreChildren]
        public virtual IEnumerable<GalleryItem> Gallery { get; set; }
        [SitecoreFieldAttribute(FieldId = "{762E69E8-C338-461C-AE3C-C98ECB9E8869}")]
        public virtual string CardType { get; set; }
        [SitecoreFieldAttribute(FieldId = "{96205169-A0FC-4DF2-9732-CC576C7D2510}")]
        public virtual string MediaType { get; set; }

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
        [SitecoreFieldAttribute(FieldId = "{D50DB906-81BA-4ECB-B727-57002B9022B0}")]
        public virtual string SubHeading { get; set; }
    }

    public class GalleryItem : ImageSourceModel
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

        [SitecoreFieldAttribute(FieldId = "{2E795C0F-28F1-4A3C-8C80-6665B4ABA7F7}")]
        public virtual string LinkTarget { get; set; }
        [SitecoreFieldAttribute(FieldId = "{1C468078-12C4-41CF-BF89-5728AE352962}")]
        public virtual GtmDetails GtmData { get; set; }

        [SitecoreChildren]
        public virtual IEnumerable<HoverDetail> HoverDetails { get; set; }
        public virtual string Badge { get; set; }
        [SitecoreFieldAttribute(FieldId = "{AEF7AA36-A693-4A85-9A79-74E90ABF961C}")]
        public virtual string Heading { get; set; }

        [SitecoreFieldAttribute(FieldId = "{EA82288C-5154-4095-A205-0E89A6F2AD55}")]
        public virtual string Description { get; set; }

    }


    public class HoverDetail
    {
        [SitecoreFieldAttribute(FieldId = "{9A242C46-61ED-4095-B255-6B7494012F16}")]
        public virtual  int Id { get; set; }
        //[SitecoreFieldAttribute(FieldId = "{1ECB9C63-E8F6-4C16-9E87-01E899AE997A}")]
        //public virtual string HoverImage { get; set; }
        [SitecoreFieldAttribute(FieldId = "{FB87C5CA-4785-4194-AADA-6599979DB2EE}")]
        public virtual Image HoverImageUrl { get; set; }
        public string HoverImage
        {
            get
            {
                return HoverImageUrl?.Src;
            }

        }
        [SitecoreFieldAttribute(FieldId = "{60EB22A6-4091-469B-80ED-3AFA30E43A75}")]
        public virtual string Alt { get; set; }
        [SitecoreFieldAttribute(FieldId = "{DABAD0B3-4845-4E04-A16C-7FC8A48DEC3E}")]
        public virtual string HoverText { get; set; }
    }
}