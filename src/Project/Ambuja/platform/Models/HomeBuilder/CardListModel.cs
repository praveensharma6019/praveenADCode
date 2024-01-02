using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Configuration.Fluent;
using Glass.Mapper.Sc.Fields;
using Project.AmbujaCement.Website.Models.Common;
using System.Collections.Generic;
using Link = Glass.Mapper.Sc.Fields.Link;

namespace Project.AmbujaCement.Website.Models.HomeBuilder
{
    public class CardListModel 
    {
        [SitecoreChildren]
        public virtual IEnumerable<CardListItem> Cards { get; set; }
    }

    public class CardListItem : ImageSourceModel
    {
        public virtual string Heading { get; set; }
        public virtual string Description { get; set; }
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
        public virtual string LinkText { get; set; }
        public virtual string Readless { get; set; }

        [SitecoreFieldAttribute(FieldId = "{A9E4F70E-B905-48D7-A0EB-976B9F9C9DA2}")]
        public virtual GtmDetails gtmData { get; set; }
        public virtual bool IsReadMore { get; set; }
    }
}