using Adani.EV.Project.Models.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.EV.Project.Models
{
    public class FooterNavigationModel : IId, ITitle, IType, IImage, ICtaLink, ICtaText
    {
        public string Id { get; set; }
        public string Imagesrc { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string CtaLink { get; set; }
        public string CtaText { get; set; }

        public List<FooterNavigationWidgetModel> widgetItems { get; set; } = new List<FooterNavigationWidgetModel>();
    }

    public class FooterNavigationWidgetModel : IId, ITitle, ISubTitle, IImage
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Imagesrc { get; set; }

        public List<FooterNavigationWidgetItemModel> items { get; set; } = new List<FooterNavigationWidgetItemModel>();
    }

    public class FooterNavigationWidgetItemModel : IId, ICtaLink, ICtaText
    {
        public string Id { get; set; }
        public string CtaLink { get; set; }
        public string CtaText { get; set; }

    }
}