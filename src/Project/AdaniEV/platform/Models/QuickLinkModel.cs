using Adani.EV.Project.Models.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.EV.Project.Models
{
    public class QuickLinkModel :IId, ITitle,IType
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }  
        public List<QuickLinkWidgetItem> widgetItems { get; set; }
    }

    public class QuickLinkWidgetItem : IId, ITitle, ISubTitle, IImage,ICtaLink, ICtaText
    { 
        public string Id { get; set; }
        public string Imagesrc { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string CtaText { get; set; }
        public string CtaLink { get; set; }
    }
}