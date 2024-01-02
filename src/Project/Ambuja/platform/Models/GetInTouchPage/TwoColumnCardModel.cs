using Glass.Mapper.Sc.Configuration.Attributes;
using Project.AmbujaCement.Website.Models.Common;
using System.Collections.Generic;
using Link = Glass.Mapper.Sc.Fields.Link;

namespace Project.AmbujaCement.Website.Models.GetInTouchPage
{

    [SitecoreType(TemplateId = "{DA16883C-3744-451D-B85C-9E0244BA5113}")]
    public class TwoColumnCardModel
    {
        [SitecoreFieldAttribute(FieldId = "{E23E26B1-6871-41CA-85A9-15FFF9BF5019}")]
        public virtual string Variant { get; set; }

        [SitecoreFieldAttribute(FieldId = "{782B0141-674E-41E2-BA9C-641E26B56957}")]
        public virtual string Theme { get; set; }
        [SitecoreFieldAttribute(FieldId = "{434485EF-FE09-4440-8EF6-5E093A6723D5}")]
        public virtual string SectionID { get; set; }

        [SitecoreChildren]
        public virtual IEnumerable<TwoColumnCardDataModel> Data { get; set; }
    }

    public class TwoColumnCardDataModel : ImageSourceModel
    {
        [SitecoreFieldAttribute(FieldId = "{CC913CEC-BE6F-4D2B-A273-99063D67A052}")]
        public virtual bool TextFirst { get; set; }

        [SitecoreFieldAttribute(FieldId = "{762E69E8-C338-461C-AE3C-C98ECB9E8869}")]
        public virtual string CardType { get; set; }

        [SitecoreFieldAttribute(FieldId = "{96205169-A0FC-4DF2-9732-CC576C7D2510}")]
        public virtual string MediaType { get; set; }

        [SitecoreFieldAttribute(FieldId = "{AEF7AA36-A693-4A85-9A79-74E90ABF961C}")]
        public virtual string Heading { get; set; }

        [SitecoreFieldAttribute(FieldId = "{D50DB906-81BA-4ECB-B727-57002B9022B0}")]
        public virtual string SubHeading { get; set; }

        [SitecoreFieldAttribute(FieldId = "{EA82288C-5154-4095-A205-0E89A6F2AD55}")]
        public virtual string Description { get; set; }

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

        [SitecoreFieldAttribute(FieldId = "{A411F701-5EFD-4886-87AF-2A846BD18AFA}")]
        public virtual string LinkText { get; set; }

        [SitecoreFieldAttribute(FieldId = "{E5141247-CE3D-4A2F-AC25-228A9286A796}")]
        public virtual string ImageTitle { get; set; }

        [SitecoreFieldAttribute(FieldId = "{D8468A04-C1D8-498A-B97A-74D7F557907B}")]
        public virtual GtmDetails gtmData { get; set; }
        public virtual string ReadMore { get; set; }
        public virtual string ReadLess { get; set; }
        public virtual bool IsReadMore { get; set; }
        
    }
}