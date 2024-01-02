using Adani.EV.Project.Models.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.EV.Project.Models
{
    public class WhySearchWithUsModel : IId, ITitle,IType
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }       
        public List<WhySearchWithUsWidgetItem> widgetItems { get; set; }
    }

    public class WhySearchWithUsWidgetItem:IId, ITitle, ISubTitle, IImage
    {
        public string Id { get; set; }
        public string Imagesrc { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
    }
}