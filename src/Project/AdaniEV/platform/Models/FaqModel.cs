using Adani.EV.Project.Models.BaseModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.EV.Project.Models
{
    public class FaqModel : IId, ITitle, IType, ICtaLink, ICtaText
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string CtaText { get; set; }
        public string CtaLink { get; set; }
        public string Type { get; set; }
        public List<FaqWidgetItems> widgetItems { get; set; }
    }

  

   
}