using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adani.EV.Project.Models.BaseModel;

namespace Adani.EV.Project.Models
{
    public class LanguageModel : IId
    {      
        public string Id { get; set; }        
        public List<LanguageWidgetModel> widgetItems { get; set; }
    }

    public class LanguageWidgetModel : IId, ICtaText, ICtaLink, IImage, ITitle
    {     
        public string Id { get; set; }  
        public string Imagesrc { get; set; }   
        public string CtaText { get; set; }     
        public string CtaLink { get; set; }
        public string Title { get; set; }
    }
}