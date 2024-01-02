using Glass.Mapper.Sc.Fields;
using Project.Mining.Website.Models.Common;

namespace Project.Mining.Website.Models.Home
{
    public class SubscribeUsModel : ImageModel
    {
        public virtual string Description { get; set; }
        public virtual string Heading { get; set; }
        public virtual Link Link { get; set; }
    }
}