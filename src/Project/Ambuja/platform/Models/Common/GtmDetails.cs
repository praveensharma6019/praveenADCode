using Glass.Mapper.Sc.Configuration.Attributes;

namespace Project.AmbujaCement.Website.Models.Common
{
    [SitecoreType(TemplateId = "{7326D615-76A0-4D1C-8251-04D4DBDDE19F}")]
    public class GtmDetails
    {
        [SitecoreFieldAttribute(FieldId = "{C35C757D-CF8C-4162-801A-9315B612FBAC}")]
        public virtual string Event { get; set; }

        [SitecoreFieldAttribute(FieldId = "{FCBD53CB-90CB-4943-B71C-D351B98F628D}")]
        public virtual string Category { get; set; }

        [SitecoreFieldAttribute(FieldId = "{DED14A3D-EE1D-4B1F-85A6-7EF825CFAF6A}")]
        public virtual string Sub_category { get; set; }

        [SitecoreFieldAttribute(FieldId = "{199F7F2A-AF83-4807-87AB-2BE654629755}")]
        public virtual string Label { get; set; }

        [SitecoreFieldAttribute(FieldId = "{EF47B038-9409-41F2-941F-21426387A79B}")]
        public virtual string Page_Type { get; set; }

        [SitecoreFieldAttribute(FieldId = "{05E16BF6-D387-452B-ADE1-F2763EACA635}")]
        public virtual string title { get; set; }
    }
}