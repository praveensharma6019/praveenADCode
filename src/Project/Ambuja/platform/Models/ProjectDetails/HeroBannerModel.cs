using Glass.Mapper.Sc.Configuration.Attributes;
using Project.AmbujaCement.Website.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Link = Glass.Mapper.Sc.Fields.Link;

namespace Project.AmbujaCement.Website.Models.ProjectDetails
{
    public class HeroBannerModel : ImageSourceModel
    {
        [SitecoreFieldAttribute(FieldId = "{E23E26B1-6871-41CA-85A9-15FFF9BF5019}")]
        public virtual string Variant { get; set; }

        [SitecoreFieldAttribute(FieldId = "{AB1FF816-4140-44D0-9318-89D1DA35C3B5}")]
        public virtual ProductData ProductData { get; set; }

        [SitecoreChildren]
        public virtual IEnumerable<SocialIcon> SocialIcons { get; set; }
    }

    [SitecoreType(TemplateId = "{371BE5E4-893B-4DB0-9B16-1C18F3CE622D}")] 
    public class ProductData :  ImageSourceModel
    {
        [SitecoreFieldAttribute(FieldId = "{D50DB906-81BA-4ECB-B727-57002B9022B0}")]
        public virtual string SubHeading { get; set; }

        [SitecoreFieldAttribute(FieldId = "{AEF7AA36-A693-4A85-9A79-74E90ABF961C}")]
        public virtual string Heading { get; set; }

        [SitecoreFieldAttribute(FieldId = "{EA82288C-5154-4095-A205-0E89A6F2AD55}")]
        public virtual string Description { get; set; }

        [SitecoreFieldAttribute(FieldId = "{1E31CB02-3EB9-461E-BAD6-F398DC717C72}")]
        public virtual string Itemicon { get; set; }

        [SitecoreChildren]
        public virtual IEnumerable<ButtonModel> Buttons { get; set; }
    }
    public class ButtonModel
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

        [SitecoreFieldAttribute(FieldId = "{1C468078-12C4-41CF-BF89-5728AE352962}")]
        public virtual GtmDetails GtmData { get; set; }
    }


    public class SocialIcon
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

        [SitecoreFieldAttribute(FieldId = "{756AB547-79C4-49FF-8976-BA76E56DB5D9}")]
        public virtual string Itemicon { get; set; }

        [SitecoreFieldAttribute(FieldId = "{1C468078-12C4-41CF-BF89-5728AE352962}")]
        public virtual GtmDetails GtmData { get; set; }
    }
}