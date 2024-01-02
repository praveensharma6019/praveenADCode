using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using Project.AAHL.Website.Models.Common;
using System.Collections.Generic;

namespace Project.AAHL.Website.Models.Home
{
    public class BannerAdsModel
    {
        public virtual WidgetBannerAdsModel widget { get; set; }
    }

    public class WidgetBannerAdsModel : WidgetModel
    {
        public virtual IEnumerable<BannerAdsWidgetItem> widgetItems { get; set; }
    }
    public class BannerAdsWidgetItem
    {
        [SitecoreField]
        public virtual string Title { get; set; }
        [SitecoreField]
        public virtual string SubTitle { get; set; }
        [SitecoreField]
        public virtual string Description { get; set; }
        [SitecoreField]
        public virtual Link LinkUrl { get; set; }
        [SitecoreField]
        public virtual string Target { get; set; }
        [SitecoreField]
        public virtual string BtnText { get; set; }
        [SitecoreField]
        public virtual Image ImagePath { get; set; }
        [SitecoreField]
        public virtual Image MImagePath { get; set; }
        [SitecoreField]
        public virtual Image TImagePath { get; set; }
        [SitecoreField]
        public virtual string Direction { get; set; }
        [SitecoreField]
        public virtual string ItemAlignment { get; set; }
        [SitecoreField]
        public virtual int GridSize { get; set; }
    }
}