using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adani.EV.Project.Models.BaseModel;

namespace Adani.EV.Project.Models
{
    public class CopyrightModel : IId
    {      
        public string Id { get; set; }        
        public List<CopyrightWidgetItem> widgetItems { get; set; }
    }

    public class CopyrightWidgetItem : IId, ICtaText, ICtaLink, ITitle
    {     
        public string Id { get; set; }  
        public string Title { get; set; }   
        public string CtaText { get; set; }     
        public string CtaLink { get; set; }
    }
}