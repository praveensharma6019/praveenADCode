using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;

namespace Project.AdaniOneSEO.Website.Models.FlightsToChennai
{
    public class PageMetaDataModel
    {
        public virtual string MetaTitle { get; set; }
        public virtual string MetaDescription { get; set; }
        public virtual string Keywords { get; set; }
        public virtual string Viewport { get; set; }
        public virtual string Robots { get; set; }
        public virtual string OG_Title { get; set; }
        public virtual Image OG_Image { get; set; }
        public virtual string OG_Description { get; set; }

        [SitecoreFieldAttribute(FieldId = "{10133F53-CBFF-4C7A-B180-3B33DE15170A}")]
        public virtual Link CanonicalUrl { get; set; }
        public string Canonical
        {
            get
            {
                return CanonicalUrl?.Url;
            }

        }
    }
}