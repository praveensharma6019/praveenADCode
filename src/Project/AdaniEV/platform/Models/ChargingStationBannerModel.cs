using Adani.EV.Project.Models.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.EV.Project.Models
{
    public class ChargingStationBannerModel
    {
        public List<ChargingStationBannerWidgetItem> widgetItems { get; set; }
    }

    public class ChargingStationBannerWidgetItem : IId, ITitle, IImage, ICtaLink, ICtaText
    {
        public string Id { get; set; }
        public string Imagesrc { get; set; }
        public string Title { get; set; }
        public string CtaLink { get; set; }
        public string CtaText { get; set; }
    }
}