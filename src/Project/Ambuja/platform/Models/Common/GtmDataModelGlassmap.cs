using Glass.Mapper.Sc.Configuration.Attributes;

namespace Project.AmbujaCement.Website.Models
{
    //[SitecoreType(TemplateId = "{A7FE2C78-BDE0-4180-A165-F4ED61D8108C}", AutoMap = true)]
    public class GtmDataModelGlassmap
    {
        
        public virtual string Event { get; set; }
        public virtual string Category { get; set; }
        public virtual string Banner_category { get; set; }
        public virtual string Title { get; set; }
         public virtual string Section_title { get; set; }
        public virtual string Label { get; set; }
        public virtual string Index { get; set; }
        public virtual string Page_type { get; set; }
        public virtual string Sub_category { get; set; }
        public virtual string Click_text { get; set; }
    }
}