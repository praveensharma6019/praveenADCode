using Glass.Mapper.Sc.Configuration.Attributes;
using Project.AmbujaCement.Website.Models.Common;
using System.Collections.Generic;

namespace Project.AmbujaCement.Website.Models.AboutUsPage
{
    [SitecoreType(TemplateId = "{16E8C3A1-E667-4447-9635-8D7395E53EED}")]
    public class MessageCardModel
    {
        [SitecoreFieldAttribute(FieldId = "{E23E26B1-6871-41CA-85A9-15FFF9BF5019}")]
        public virtual string Variant { get; set; }

        [SitecoreFieldAttribute(FieldId = "{434485EF-FE09-4440-8EF6-5E093A6723D5}")]
        public virtual string SectionID { get; set; }

        [SitecoreChildren]
        public virtual IEnumerable<MessageCardDetails> Data { get; set; }
    }

    public class MessageCardDetails : ImageSourceModel
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

        [SitecoreFieldAttribute(FieldId = "{8710E4E8-5371-4A97-8B5A-C883A0C1F26F}")]
        public virtual string SubDescription { get; set; }
    }
}