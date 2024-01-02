using Adani.EV.Project.Models.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.EV.Project.Models
{
    public class QuickInfoModel : IId, ITitle,IType, IBackgroundColor
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string BackgroundColor { get; set; }
        public List<QuickInfoWidgetItem> widgetItems { get; set; }
    }

    public class QuickInfoWidgetItem : IId, ITitle, ISubTitle, IImage
    {
        public string Id { get; set; }
        public string Imagesrc { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
    }
}