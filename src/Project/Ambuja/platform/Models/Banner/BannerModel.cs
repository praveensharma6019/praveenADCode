using Project.AmbujaCement.Website.Models.Common;

namespace Project.AmbujaCement.Website.Models
{
    public class BannerModel : ImageSourceModel
    {
        public virtual string Description { get; set; }
        public virtual string Variant { get; set; }
        public virtual string Heading { get; set; }
        public virtual string SubHeading { get; set; }
    }
}