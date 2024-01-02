using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adani.EV.Project.Models.BaseModel;

namespace Adani.EV.Project.Models
{
    public class EVNearBannerModel : IId
    {
        public string Id { get; set; }
        public List<EVNearBannerWidgetItem> widgetItems { get; set; }
    }

    public class EVNearBannerWidgetItem : IId, ITitle, ISubTitle, IImage
    {
        public string Id { get; set; }
        public string Imagesrc { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
    }
}