
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using Project.AAHL.Website.Models.Home;
using Sitecore.Publishing.Pipelines.PublishVersion;
using System.Collections.Generic;

namespace Project.AAHL.Website.Models.Common
{
    public class BannerModel 
    {
        [SitecoreChildren]
        public virtual IEnumerable<BannerDetails> Items { get; set; }
    }
    public class BannerDetails : ImageModel
    {
        public virtual string Title { get; set; }

        public virtual string Description { get; set; }

        public virtual Link LinkUrl { get; set; }

        public virtual string BtnText { get; set; }

        public virtual string Direction { get; set; }

    }
}