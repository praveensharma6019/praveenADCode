using Glass.Mapper.Sc.Configuration.Attributes;
using System.Collections.Generic;

namespace Project.AmbujaCement.Website.Models.GetInTouchPage
{
    [SitecoreType(TemplateId = "{011F1452-B583-4346-BEC1-2699C2143DC3}")]
    public class GetInTouchDetailsModel
    {
        [SitecoreFieldAttribute(FieldId = "{AEF7AA36-A693-4A85-9A79-74E90ABF961C}")]
        public virtual string Heading { get; set; }

        [SitecoreFieldAttribute(FieldId = "{8DA38F2E-3807-468D-8395-FBCB65183684}")]
        public virtual string SectionHeading { get; set; }

        [SitecoreFieldAttribute(FieldId = "{D50DB906-81BA-4ECB-B727-57002B9022B0}")]
        public virtual string SubHeading { get; set; }

        [SitecoreChildren]
        public virtual IEnumerable<ContactDetails> Items { get; set; }

        [SitecoreField(FieldId = "{7772232C-0A75-49F9-9B69-C6FB6F2F331A}")]
        public virtual TextData TextData { get; set; }
    }

    public class ContactDetails
    {

        [SitecoreFieldAttribute(FieldId = "{AEF7AA36-A693-4A85-9A79-74E90ABF961C}")]
        public virtual string Heading { get; set; }

        [SitecoreFieldAttribute(FieldId = "{D50DB906-81BA-4ECB-B727-57002B9022B0}")]
        public virtual string SubHeading { get; set; }

        [SitecoreFieldAttribute(FieldId = "{B117241E-E6B5-4B36-BBF3-D674D4BF804A}")]
        public virtual string Area { get; set; }

        [SitecoreFieldAttribute(FieldId = "{A0C8CC60-7458-4635-8E89-97A5587677CF}")]
        public virtual string City { get; set; }

        [SitecoreFieldAttribute(FieldId = "{4378676E-CB60-4EC1-9F57-27EDABA9B4AD}")]
        public virtual string IconName { get; set; }
    }

    [SitecoreType(TemplateId = "{113671E3-A00B-4B23-AE35-3FED4685FBA6}")]
    public class TextData
    {
        [SitecoreFieldAttribute(FieldId = "{AEF7AA36-A693-4A85-9A79-74E90ABF961C}")]
        public virtual string Heading { get; set; }

        [SitecoreFieldAttribute(FieldId = "{EA82288C-5154-4095-A205-0E89A6F2AD55}")]
        public virtual string Description { get; set; }
    }
}